﻿using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Models;

namespace Search.CogServices;

public class AcmeSearchIndexService : IAcmeSearchIndexService
{
    private readonly SearchClientOptions _clientOptions;
    private SearchIndexClient? _client;

    /// <summary>Constructor</summary>
    public AcmeSearchIndexService(AcmeSearchSettings settings, IAcmeOptionsService optionsService)
    {
        Settings = settings;
        _clientOptions = optionsService.CreateSearchClientOptions();
    }

    /// <summary>This is the Microsoft client that does all the work.</summary>
    public SearchIndexClient Client => _client ??= new SearchIndexClient(
        new Uri(Settings.SearchEndPoint), new AzureKeyCredential(Settings.SearchApiKey), _clientOptions);

    protected AcmeSearchSettings Settings { get; private set; }

    /// <summary>Retrieves a single document.</summary>
    /// <param name="indexName">The name of the index</param>
    /// <param name="searchText">The partial bit of text to search upon</param>
    /// <param name="suggesterName">The name of the suggester</param>
    /// <param name="options">Options that allow specifying autocomplete behaviors, like fuzzy matching.</param>
    /// <param name="cancellationToken">Optional <see cref="CancellationToken"/> to propagate notifications that the operation should be canceled. </param>
    public async Task<Response<AutocompleteResults>> AutocompleteAsync(string indexName, string searchText, string suggesterName,
        AutocompleteOptions? options = null, CancellationToken cancellationToken = default)
    {
        var searchClient = Client.GetSearchClient(indexName);
        return await searchClient.AutocompleteAsync(searchText, suggesterName, options, cancellationToken);
    }

    /// <summary>Deletes an index.</summary>
    /// <param name="indexName">The name of the index to delete</param>
    public async Task<bool> DeleteAsync(string indexName)
    {
        if (await ExistsAsync(indexName) == false)
            return true; // it does not exists

        await Client.DeleteIndexAsync(indexName);

        return true;
    }

    /// <summary>Indicates if an index exists.</summary>
    /// <param name="indexName">The name of the index to find.</param>
    /// <remarks>Unfortunately, we get this response as an exception from the API,
    /// so we have to check for the HTTP status code of 404 to determine if it was really missing or if there was
    /// some type of other error</remarks>
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

    /// <summary>Retrieves a single document.</summary>
    /// <typeparam name="T">The type of data being returned.</typeparam>
    /// <param name="indexName">The name of the index</param>
    /// <param name="key">The documents key</param>
    public async Task<Response<T>?> GetDocumentAsync<T>(string indexName, string key)
    {
        try
        {
            var searchClient = Client.GetSearchClient(indexName);
            return await searchClient.GetDocumentAsync<T>(key);
        }
        catch (RequestFailedException ex)
        {
            if (ex.Status == 404)
                return null;
            throw;
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

    /// <summary>Performs a search against the index.</summary>
    /// <typeparam name="T">The type of data being returned.</typeparam>
    /// <param name="indexName">The name of the index</param>
    /// <param name="searchText">The text to find</param>
    /// <param name="options">The search options to apply</param>
    /// <param name="cancellationToken">Optional <see cref="CancellationToken"/> to propagate notifications that the operation should be canceled. </param>
    public async Task<Response<SearchResults<T>>> SearchAsync<T>(string indexName, string searchText,
        SearchOptions? options = null, CancellationToken cancellationToken = default)
    {
        var searchClient = Client.GetSearchClient(indexName);
        var response = await searchClient.SearchAsync<T>(searchText, options, cancellationToken);
        return response;
    }

    /// <summary>Used for autocomplete to get a suggestion.</summary>
    /// <typeparam name="T">The type of data being returned.</typeparam>
    /// <param name="indexName">The name of the index</param>
    /// <param name="searchText">The text to find</param>
    /// <param name="suggesterName">The name of the suggestor</param>
    /// <param name="options">The search options to apply</param>
    /// <param name="cancellationToken"></param>
    public async Task<SuggestResults<T>> SuggestAsync<T>(string indexName, string searchText, string suggesterName,
        SuggestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var searchClient = Client.GetSearchClient(indexName);
        return await searchClient.SuggestAsync<T>(searchText, suggesterName, options, cancellationToken);
    }

    /// <summary>Uploads documents to an index.</summary>
    /// <typeparam name="T">The class type that we are uploading.</typeparam>
    /// <param name="indexName">The name of the index</param>
    /// <param name="uploadList">The list of items of type T to upload.</param>
    public async Task UploadDocuments<T>(string indexName, List<T> uploadList)
    {
        if (uploadList.Count == 0) return;

        // Turn the uploadLIst into an array of Upload Actions
        IndexDocumentsAction<T>[] actions = uploadList.Select(s => IndexDocumentsAction.Upload(s)).ToArray();

        // Create a back of actions
        IndexDocumentsBatch<T> batch = IndexDocumentsBatch.Create(actions);

        SearchClient searchClient = Client.GetSearchClient(indexName);

        IndexDocumentsResult result = await searchClient.IndexDocumentsAsync(batch);
    }
}