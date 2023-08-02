﻿using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using Azure.Search.Documents.Models;
using CustomSqlServerIndexer.Models;
using System.Text.Json;

namespace CustomSqlServerIndexer.Services;

public class CustomSearchIndexService : ICustomSearchIndexService
{
    private readonly ServiceSettings _settings;
    private SearchIndexClient? _indexClient;
    private SearchClient? _searchClient;

    /// <summary>
    /// Constructor
    /// </summary>
    public CustomSearchIndexService(ServiceSettings settings)
    {
        _settings = settings;
    }


    /// <summary>
    /// Clears all documents from the index.
    /// </summary>
    /// <param name="keyField">The name of the key field that uniquely identifies documents in the index.</param>
    /// <returns>The number of documents deleted</returns>
    public async Task<long> DeleteAllDocumentsAsync(string keyField)
    {
        try
        {
            var searchClient = GetSearchClient();

            long totalDeleted = 0;
            long totalCountOnLastTry = 0;
            long totalCountCurrent;

            do
            {
                var options = new SearchOptions
                {
                    Size = 50,
                    QueryType = SearchQueryType.Simple,
                    IncludeTotalCount = true,
                    Select = { keyField }
                };

                var azSearchResults = await this.SearchAsync<SearchDocument>("*", options);

                totalCountCurrent = azSearchResults.TotalCount ?? 0;

                if (totalCountCurrent > 0)
                {
                    if (totalCountCurrent == totalCountOnLastTry)
                    {
                        // We are stuck and docs aren't be deleted!
                        break;
                    }

                    var keys = new List<string>();
                    foreach (var doc in azSearchResults.Docs)
                    {
                        keys.Add(doc.Document.GetString(keyField));
                    }

                    await searchClient.DeleteDocumentsAsync(keyField, keys);

                    totalDeleted += azSearchResults.Docs.Count;
                }

                totalCountOnLastTry = totalCountCurrent;

            } while (totalCountCurrent > 0);

            return totalDeleted;
        }
        catch (RequestFailedException ex)
        {
            if (ex.Status == 404)
                return 0;
            throw;
        }
    }

    /// <summary>
    /// Clears the specified documents from the index.
    /// </summary>
    /// <param name="keyField">The name of the key field that uniquely identifies documents in the index.</param>
    /// <param name="keys">The keys of the documents to delete.</param>
    /// <returns>The number of documents deleted</returns>
    public async Task<long> DeleteDocumentsAsync(string keyField, List<string> keys)
    {
        if (keys.Count == 0)
            return 0;

        try
        {
            var searchClient = GetSearchClient();
            await searchClient.DeleteDocumentsAsync(keyField, keys);

            return keys.Count;
        }
        catch (RequestFailedException ex)
        {
            if (ex.Status == 404)
                return 0;
            throw;
        }
    }

    /// <summary>
    /// Create index or update the index.
    /// </summary>
    public void CreateOrUpdateIndex()
    {
        FieldBuilder fieldBuilder = new FieldBuilder();
        var searchFields = fieldBuilder.Build(typeof(SearchIndexDocument));

        var definition = new SearchIndex(_settings.CognitiveSearchIndexName, searchFields);

        // setup the suggestor
        string hotelNameFieldName = JsonNamingPolicy.CamelCase.ConvertName(nameof(SearchIndexDocument.HotelName));
        string categoryFieldName = JsonNamingPolicy.CamelCase.ConvertName(nameof(SearchIndexDocument.Category));
        var suggester = new SearchSuggester("sg", new[] { hotelNameFieldName, categoryFieldName });
        definition.Suggesters.Add(suggester);


        // Setup Semantic Configuration
        var prioritizedFields = new PrioritizedFields()
        {
            TitleField = new SemanticField()
            {
                FieldName = nameof(SearchIndexDocument.HotelName)
            }
        };

        prioritizedFields.ContentFields.Add(new SemanticField() { FieldName = nameof(SearchIndexDocument.HotelName) });
        prioritizedFields.KeywordFields.Add(new SemanticField() { FieldName = nameof(SearchIndexDocument.Description) });

        SemanticConfiguration semanticConfig = new SemanticConfiguration(_settings.CognitiveSearchSemanticConfigurationName, prioritizedFields);
        definition.SemanticSettings = new SemanticSettings();
        definition.SemanticSettings.Configurations.Add(semanticConfig);

        // Create it using the index client
        var indexClient = GetIndexClient();
        indexClient.CreateOrUpdateIndex(definition);
    }

    /// <summary>Searches for documents</summary>
    /// <typeparam name="T">The type of data being returned.</typeparam>
    /// <param name="searchText">The text to find</param>
    /// <param name="options">The search options to apply</param>
    public async Task<SearchQueryResponse<T>> SearchAsync<T>(string searchText, SearchOptions options) where T : class
    {
        var searchClient = GetSearchClient();
        var response = await searchClient.SearchAsync<T>(searchText, options);

        var result = new SearchQueryResponse<T>
        {
            Docs = await response.Value.ToSearchResultDocumentsAsync(),
            TotalCount = response.Value.TotalCount
        };

        return result;
    }

    /// <summary>
    /// Upload documents in a single Upload request.
    /// </summary>
    /// <param name="doc"></param>
    public void UploadDocuments(SearchIndexDocument doc)
    {
        IndexDocumentsBatch<SearchIndexDocument> batch = IndexDocumentsBatch.Create(
            IndexDocumentsAction.Upload(doc));

        var searchClient = GetSearchClient();
        IndexDocumentsResult result = searchClient.IndexDocuments(batch);
    }

    private SearchIndexClient GetIndexClient()
    {
        if (_indexClient == null)
        {
            var serviceEndpoint = GetServiceEndpoint();
            var credential = new AzureKeyCredential(_settings.CognitiveSearchKey);
            _indexClient = new SearchIndexClient(serviceEndpoint, credential);
        }

        return _indexClient;
    }

    private SearchClient GetSearchClient()
    {
        if (_searchClient == null)
        {
            var serviceEndpoint = GetServiceEndpoint();
            var credential = new AzureKeyCredential(_settings.CognitiveSearchKey);
            _searchClient = new SearchClient(serviceEndpoint, _settings.CognitiveSearchIndexName, credential);
        }

        return _searchClient;
    }

    private Uri GetServiceEndpoint()
    {
        Uri serviceEndpoint = new Uri($"https://{_settings.CognitiveSearchName}.search.windows.net/");
        return serviceEndpoint;
    }
}