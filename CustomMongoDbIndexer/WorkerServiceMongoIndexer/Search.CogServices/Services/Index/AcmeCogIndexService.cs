using Azure;
using Azure.Search.Documents.Models;
using Azure.Search.Documents;
using Search.CogServices.Extensions;

namespace Search.CogServices;

/// <summary>
/// Used for doing index manipulation and get, upload and delete documents.  If you are searching documents use one of the following:
/// <see cref="AcmeCogSearchService"/> or <see cref="AcmeCogSuggestService"/> or <see cref="AcmeCogAutoCompleteService"/>
/// </summary>
public class AcmeCogIndexService : IAcmeCogIndexService
{
    private readonly IAcmeCogSearchService _cogSearchService;

    /// <summary>Constructor</summary>
    public AcmeCogIndexService(AcmeCogSettings settings, IAcmeCogClientService clientService, IAcmeCogSearchService cogSearchService)
    {
        _cogSearchService = cogSearchService;
        ClientService = clientService;
        Settings = settings;
    }

    protected IAcmeCogClientService ClientService { get; }
    protected AcmeCogSettings Settings { get; private set; }


    /// <summary>
    /// Clears all documents from the index.
    /// </summary>
    /// <param name="indexName">The name of the index</param>
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

                var azSearchResults = await _cogSearchService.SearchAsync<SearchDocument>(indexName, "*", options);

                totalCountCurrent = azSearchResults.Value.TotalCount ?? 0;

                if (totalCountCurrent > 0)
                {
                    if (totalCountCurrent == totalCountOnLastTry)
                    {
                        // We are stuck and docs aren't be deleted!
                        break;
                    }

                    var keys = new List<string>();
                    var docs = await azSearchResults.Value.ToSearchResultDocumentsAsync();
                    foreach (var doc in docs)
                    {
                        keys.Add(doc.Document.GetString(keyField));
                    }

                    await searchClient.DeleteDocumentsAsync(keyField, keys);

                    totalDeleted += docs.Count;
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

    /// <summary>Deletes an index.</summary>
    /// <param name="indexName">The name of the index to delete</param>
    public async Task<bool> DeleteAsync(string indexName)
    {
        if (await ExistsAsync(indexName) == false)
            return true; // it does not exists

        var indexClient = ClientService.GetIndexClient();
        await indexClient.DeleteIndexAsync(indexName);

        return true;
    }

    /// <summary>
    /// Clears the specified documents from the index.
    /// </summary>
    /// <param name="indexName">The name of the index</param>
    /// <param name="keyField">The name of the key field that uniquely identifies documents in the index.</param>
    /// <param name="key">The key (primary field) of the document to delete.</param>
    /// <returns>The number of documents deleted</returns>
    public async Task<bool> DeleteDocumentAsync(string indexName, string keyField, string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            return false;

        var count = await DeleteDocumentsAsync(indexName, keyField, new List<string> { key });

        return count > 0;
    }

    /// <summary>
    /// Clears the specified documents from the index.
    /// </summary>
    /// <param name="indexName">The name of the index</param>
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

    /// <summary>Indicates if an index exists.</summary>
    /// <param name="indexName">The name of the index to find.</param>
    /// <remarks>Unfortunately, we get this response as an exception from the API,
    /// so we have to check for the HTTP status code of 404 to determine if it was really missing or if there was
    /// some type of other error</remarks>
    public async Task<bool> ExistsAsync(string indexName)
    {
        try
        {
            var indexClient = ClientService.GetIndexClient();
            return await indexClient.GetIndexAsync(indexName) != null;
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
            var searchClient = ClientService.GetSearchClient(indexName);
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

        var indexClient = ClientService.GetIndexClient();
        AsyncPageable<string>? pages = indexClient.GetIndexNamesAsync();

        await foreach (Page<string> onePage in pages.AsPages())
        {
            foreach (string oneIndexName in onePage.Values)
            {
                result.Add(oneIndexName);
            }
        }

        return result;
    }
  
    /// <summary>
    /// Uploads one document to an index.
    /// </summary>
    /// <param name="indexName">The name of the index</param>
    /// <param name="doc">One document</param>
    public async Task<IndexDocumentsResult> UploadDocumentAsync<T>(string indexName, T doc)
    {
        IndexDocumentsAction<T>? actions = IndexDocumentsAction.Upload(doc);

        IndexDocumentsBatch<T> batch = IndexDocumentsBatch.Create(actions);

        var searchClient = this.ClientService.GetSearchClient(indexName);

        IndexDocumentsResult result = await searchClient.IndexDocumentsAsync(batch);

        return result;
    }

    /// <summary>Uploads documents to an index.</summary>
    /// <typeparam name="T">The class type that we are uploading.</typeparam>
    /// <param name="indexName">The name of the index</param>
    /// <param name="documentList">The list of items of type T to upload.</param>
    public async Task<IndexDocumentsResult> UploadDocuments<T>(string indexName, List<T> documentList)
    {

        // Turn the uploadLIst into an array of Upload Actions
        IndexDocumentsAction<T>[] actions = documentList.Select(s => IndexDocumentsAction.Upload(s)).ToArray();

        // Create a back of actions
        IndexDocumentsBatch<T> batch = IndexDocumentsBatch.Create(actions);

        SearchClient searchClient = ClientService.GetSearchClient(indexName);

        IndexDocumentsResult result = await searchClient.IndexDocumentsAsync(batch);

        return result;
    }

}