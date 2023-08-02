using Azure.Search.Documents;
using CustomBlobIndexer.Models;

namespace CustomBlobIndexer.Services;

public interface ICustomSearchIndexService
{
    /// <summary>
    /// Clears all documents from the index.
    /// </summary>
    /// <param name="keyField"></param>
    /// <returns>The number of documents deleted</returns>
    Task<long> DeleteAllDocumentsAsync(string keyField);

    /// <summary>
    /// Clears the specified documents from the index.
    /// </summary>
    /// <param name="keyField">The name of the key field that uniquely identifies documents in the index.</param>
    /// <param name="keys">The keys of the documents to delete.</param>
    /// <returns>The number of documents deleted</returns>
    Task<long> DeleteDocumentsAsync(string keyField, List<string> keys);


    /// <summary>
    /// Create index or update the index.
    /// </summary>
    void CreateOrUpdateIndex();

    /// <summary>Searches for documents</summary>
    /// <typeparam name="T">The type of data being returned.</typeparam>
    /// <param name="searchText">The text to find</param>
    /// <param name="options">The search options to apply</param>
    Task<SearchQueryResponse<T>> SearchAsync<T>(string searchText, SearchOptions options) where T : class;

    /// <summary>
    /// Upload documents in a single Upload request.
    /// </summary>
    /// <param name="doc"></param>
    void UploadDocuments(SearchIndexDocument doc);
}