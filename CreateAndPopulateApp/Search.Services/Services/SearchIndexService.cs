﻿using System.Text.Json;
using Azure;
using Azure.Core.Serialization;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using Azure.Search.Documents.Models;

namespace Search.Services;

public class SearchIndexService : ISearchIndexService
{
    private readonly SearchServiceSettings _settings;
    private SearchIndexClient? _client;
    private readonly SearchClientOptions _clientOptions;

    /// <summary>Constructor</summary>
    public SearchIndexService(SearchServiceSettings settings)
    {
        _settings = settings;
        _clientOptions =  CreateSearchClientOptions();
    }
    

    public SearchIndexClient Client => _client ??= new SearchIndexClient(new Uri(_settings.SearchEndPoint), new AzureKeyCredential(_settings.SearchAdminApiKey), _clientOptions);

    public async Task<bool> CreateOrUpdateAsync(Type typeToCreate, string indexName)
    {
        FieldBuilder fieldBuilder = new FieldBuilder();
        var searchFields = fieldBuilder.Build(typeToCreate);
        var searchIndex = new SearchIndex(indexName, searchFields);

        Response<SearchIndex>? result = await Client.CreateOrUpdateIndexAsync(searchIndex);

        return result != null && result.Value != null;
    }

    public async Task<bool> DeleteAsync(string indexName)
    {
        if (await ExistsAsync(indexName) == false)
            return true; // it does not exists

        await Client.DeleteIndexAsync(indexName);

        return true;
    }

    public async Task<bool> ExistsAsync(string indexName)
    {
        try
        {
            return await Client.GetIndexAsync(indexName) != null;
        }
        catch (RequestFailedException e) when (e.Status == 404)
        {
            // if exception occurred and status is "Not Found", this is working as expected
            // because someone was too lazy to put in an exist query.
            return false;
        }
    }

    /// <summary>Returns a list of index names.</summary>
    public async Task<List<string>> GetIndexNamesAsync()
    {
        var result = new List<string>();
        AsyncPageable<string>? pages = Client.GetIndexNamesAsync();

        await foreach (Page<string> onePage in pages.AsPages())
        {
            foreach (string oneIndexName in onePage.Values)
            {
                result.Add(oneIndexName);
            }
        }

        return result;
    }

    /// <summary>Uploads documents to an index.</summary>
    /// <typeparam name="T">The class type that we are uploading.</typeparam>
    /// <param name="indexName">The name of the index</param>
    /// <param name="uploadList">The list of items of type T to upload.</param>
    public async Task UploadDocuments<T>(string indexName, List<T> uploadList)
    {
        if (uploadList.Count == 0) return;

        IndexDocumentsAction<T>[] actions = uploadList.Select(s => IndexDocumentsAction.Upload(s)).ToArray();
        IndexDocumentsBatch<T> batch = IndexDocumentsBatch.Create(actions);

        SearchClient searchClient = Client.GetSearchClient(indexName);

        IndexDocumentsResult result = await searchClient.IndexDocumentsAsync(batch);
    }

    /// <summary>Create search options</summary>
    private SearchClientOptions CreateSearchClientOptions()
    {
        // This is needed to avoid an error when uploading data that has a GeographyPoint property.  
        // Here is the error: The request is invalid. Details: parameters : Cannot find nested property 'location' on the resource type 'search.documentFields'.
        JsonSerializerOptions serializerOptions = new JsonSerializerOptions
        {
            Converters =
            {
                // Requires Microsoft.Azure.Core.Spatial NuGet package.
                new MicrosoftSpatialGeoJsonConverter()
            },
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        return new SearchClientOptions
        {
            Serializer = new JsonObjectSerializer(serializerOptions)
        };
    }
}