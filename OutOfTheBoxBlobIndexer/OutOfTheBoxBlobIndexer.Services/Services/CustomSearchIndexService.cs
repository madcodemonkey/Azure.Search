﻿using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using Azure.Search.Documents.Models;
using OutOfTheBoxBlobIndexer.Models;

namespace OutOfTheBoxBlobIndexer.Services;

public class CustomSearchIndexService : ICustomSearchIndexService
{
    protected ServiceSettings ServiceSettings { get; }
    protected ICogClientWrapperService ClientService { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    public CustomSearchIndexService(ServiceSettings serviceSettings, ICogClientWrapperService clientService)
    {
        ServiceSettings = serviceSettings;
        ClientService = clientService;
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
            var searchClient = ClientService.GetSearchClient();

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
            var searchClient = ClientService.GetSearchClient();
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
    /// Deletes the entire index and all it's documents!
    /// </summary>
    /// <returns></returns>
    public async Task DeleteIndexAsync()
    {
        var indexClient = ClientService.GetIndexClient();
        await indexClient.DeleteIndexAsync(ServiceSettings.CognitiveSearchIndexName);
    }

    /// <summary>
    /// Create index or update the index.
    /// </summary>
    public void CreateOrUpdateIndex()
    {

        FieldBuilder fieldBuilder = new FieldBuilder();
        var searchFields = fieldBuilder.Build(typeof(SearchIndexDocument));

        var definition = new SearchIndex(ServiceSettings.CognitiveSearchIndexName, searchFields);

        // setup the suggestor
        var suggester = new SearchSuggester("sg", new[] { nameof(SearchIndexDocument.FirstName), nameof(SearchIndexDocument.Label) });
        definition.Suggesters.Add(suggester);


        // Setup Semantic Configuration
        var prioritizedFields = new PrioritizedFields()
        {
            TitleField = new SemanticField()
            {
                FieldName = nameof(SearchIndexDocument.FirstName)
            }
        };

        prioritizedFields.ContentFields.Add(new SemanticField() { FieldName = nameof(SearchIndexDocument.FirstName) });
        prioritizedFields.KeywordFields.Add(new SemanticField() { FieldName = nameof(SearchIndexDocument.Label) });

        SemanticConfiguration semanticConfig = new SemanticConfiguration(ServiceSettings.CognitiveSearchSemanticConfigurationName, prioritizedFields);
        definition.SemanticSettings = new SemanticSettings();
        definition.SemanticSettings.Configurations.Add(semanticConfig);

        // Create it using the index client
        var indexClient = ClientService.GetIndexClient();
        indexClient.CreateOrUpdateIndex(definition);
    }

    /// <summary>Searches for documents</summary>
    /// <typeparam name="T">The type of data being returned.</typeparam>
    /// <param name="searchText">The text to find</param>
    /// <param name="options">The search options to apply</param>
    public async Task<SearchQueryResponse<T>> SearchAsync<T>(string searchText, SearchOptions options) where T : class
    {
        var searchClient = ClientService.GetSearchClient();
        var response = await searchClient.SearchAsync<T>(searchText, options);

        var result = new SearchQueryResponse<T>
        {
            Docs = await response.Value.ToSearchResultDocumentsAsync(),
            TotalCount = response.Value.TotalCount
        };

        return result;
    }

    /// <summary>
    /// Upload one document in a single Upload request.
    /// </summary>
    /// <param name="doc">One document to upload</param>
    public async Task UploadDocumentsAsync(SearchIndexDocument doc)
    {
        IndexDocumentsBatch<SearchIndexDocument> batch = IndexDocumentsBatch.Create(
            IndexDocumentsAction.Upload(doc));

        var searchClient = ClientService.GetSearchClient();
        IndexDocumentsResult result = await searchClient.IndexDocumentsAsync(batch);
    }

    /// <summary>
    /// Upload multiple documents in a single Upload request.
    /// </summary>
    /// <param name="docs">A list of docs to upload</param>
    public async Task UploadDocumentsAsync(List<SearchIndexDocument> docs)
    {
        IndexDocumentsBatch<SearchIndexDocument> batch = IndexDocumentsBatch.Create(
            docs.Select(s => IndexDocumentsAction.Upload(s)).ToArray());

        var searchClient = ClientService.GetSearchClient();
        IndexDocumentsResult result = await searchClient.IndexDocumentsAsync(batch);
    }

}