using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using CogSearchServices.Models;

namespace CogSearchServices.Services;

public abstract class CogSearchIndexService : ICogSearchIndexService
{
    /// <summary>
    /// Constructor
    /// </summary>
    protected CogSearchIndexService(ICogSearchClientService clientService)
    {
        ClientService = clientService;
    }

    protected ICogSearchClientService ClientService { get; }


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
            var response = await searchClient.DeleteDocumentsAsync(keyField, keys);

            var numberDeleted = response?.Value.Results.Count(c => c.Succeeded) ?? 0;

            return numberDeleted;
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
    /// <param name="key">The key (primary field) of the document to delete.</param>
    /// <returns>The number of documents deleted</returns>
    public async Task<bool> DeleteDocumentAsync(string keyField, string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            return false;

        var count = await DeleteDocumentsAsync(keyField, new List<string> { key });

        return count > 0;
    }



    /// <summary>Searches for documents</summary>
    /// <typeparam name="T">The type of data being returned.</typeparam>
    /// <param name="searchText">The text to find</param>
    /// <param name="options">The search options to apply</param>
    public async Task<CogSearchQueryResponse<T>> SearchAsync<T>(string searchText, SearchOptions options) where T : class
    {
        var searchClient = ClientService.GetSearchClient();
        var response = await searchClient.SearchAsync<T>(searchText, options);

        var result = new CogSearchQueryResponse<T>
        {
            Docs = await response.Value.ToSearchResultDocumentsAsync(),
            TotalCount = response.Value.TotalCount
        };

        return result;
    }


    /// <summary>
    /// Uploads one document to an index.
    /// </summary>
    /// <param name="doc">One document</param>
    public async Task<IndexDocumentsResult> UploadDocumentAsync<T>(T doc) where T : class
    {
        IndexDocumentsAction<T>? actions = IndexDocumentsAction.Upload(doc);

        IndexDocumentsBatch<T> batch = IndexDocumentsBatch.Create(actions);

        var searchClient = this.ClientService.GetSearchClient();

        IndexDocumentsResult result = await searchClient.IndexDocumentsAsync(batch);

        return result;
    }

    /// <summary>Uploads multiple documents to an index.</summary>
    /// <typeparam name="T">The class type that we are uploading.</typeparam>
    /// <param name="documentList">The list of items of type T to upload.</param>
    public async Task<IndexDocumentsResult> UploadDocumentsAsync<T>(List<T> documentList) where T : class
    {
        // Turn the documentList into an array of Upload Actions
        IndexDocumentsAction<T>[] actions = documentList.Select(s => IndexDocumentsAction.Upload(s)).ToArray();

        // Create a back of actions
        IndexDocumentsBatch<T> batch = IndexDocumentsBatch.Create(actions);

        SearchClient searchClient = ClientService.GetSearchClient();

        IndexDocumentsResult result = await searchClient.IndexDocumentsAsync(batch);

        return result;
    }
}