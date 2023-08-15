using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using OutOfTheBoxBlobIndexer.Models;

namespace OutOfTheBoxBlobIndexer.Services;

public class CogSearchIndexService : ICogSearchIndexService
{
    protected ServiceSettings ServiceSettings { get; }
    protected ICogClientWrapperService ClientService { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    public CogSearchIndexService(ServiceSettings serviceSettings, ICogClientWrapperService clientService)
    {
        ServiceSettings = serviceSettings;
        ClientService = clientService;
    }


    /// <summary>
    /// Clears all documents from the index.
    /// </summary>
    /// <param name="indexName">The name of the index.</param>
    /// <param name="keyField">The name of the key field that uniquely identifies documents in the index.</param>
    /// <returns>The number of documents deleted</returns>
    public async Task<long> DeleteAllDocumentsAsync(string indexName, string keyField)
    {
        try
        {
            var searchClient = ClientService.GetSearchClient(indexName);

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

                var azSearchResults = await this.SearchAsync<SearchDocument>(indexName, "*", options);

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
    /// <param name="indexName">The name of the index.</param>
    /// <param name="keyField">The name of the key field that uniquely identifies documents in the index.</param>
    /// <param name="keys">The keys of the documents to delete.</param>
    /// <returns>The number of documents deleted</returns>
    public async Task<long> DeleteDocumentsAsync(string indexName, string keyField, List<string> keys)
    {
        if (keys.Count == 0)
            return 0;

        try
        {
            var searchClient = ClientService.GetSearchClient(indexName);
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
    public async Task DeleteIndexAsync(string indexName)
    {
        var indexClient = ClientService.GetIndexClient();
        await indexClient.DeleteIndexAsync(indexName);
    }

    /// <summary>Searches for documents</summary>
    /// <typeparam name="T">The type of data being returned.</typeparam>
    /// <param name="indexName">The name of the index.</param>
    /// <param name="searchText">The text to find</param>
    /// <param name="options">The search options to apply</param>
    public async Task<SearchQueryResponse<T>> SearchAsync<T>(string indexName, string searchText, SearchOptions options) where T : class
    {
        var searchClient = ClientService.GetSearchClient(indexName);
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
    /// <param name="indexName">The name of the index.</param>
    /// <param name="doc">One document to upload</param>
    public async Task UploadDocumentsAsync(string indexName, SearchIndexDocument doc)
    {
        IndexDocumentsBatch<SearchIndexDocument> batch = IndexDocumentsBatch.Create(
            IndexDocumentsAction.Upload(doc));

        var searchClient = ClientService.GetSearchClient(indexName);
        IndexDocumentsResult result = await searchClient.IndexDocumentsAsync(batch);
    }

    /// <summary>
    /// Upload multiple documents in a single Upload request.
    /// </summary>
    /// <param name="indexName">The name of the index.</param>
    /// <param name="docs">A list of docs to upload</param>
    public async Task UploadDocumentsAsync(string indexName, List<SearchIndexDocument> docs)
    {
        IndexDocumentsBatch<SearchIndexDocument> batch = IndexDocumentsBatch.Create(
            docs.Select(s => IndexDocumentsAction.Upload(s)).ToArray());

        var searchClient = ClientService.GetSearchClient(indexName);
        IndexDocumentsResult result = await searchClient.IndexDocumentsAsync(batch);
    }

}