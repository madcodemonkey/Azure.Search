using Azure.Search.Documents;
using OutOfTheBoxBlobIndexer.Models;

namespace OutOfTheBoxBlobIndexer.Services;

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
    /// Deletes the entire index and all it's documents!
    /// </summary>
    /// <returns></returns>
    Task DeleteIndexAsync();

    /// <summary>Searches for documents</summary>
    /// <typeparam name="T">The type of data being returned.</typeparam>
    /// <param name="searchText">The text to find</param>
    /// <param name="options">The search options to apply</param>
    Task<SearchQueryResponse<T>> SearchAsync<T>(string searchText, SearchOptions options) where T : class;

    /// <summary>
    /// Upload one document in a single Upload request.
    /// </summary>
    /// <param name="doc">One document to upload</param>
    Task UploadDocumentsAsync(SearchIndexDocument doc);

    /// <summary>
    /// Upload multiple documents in a single Upload request.
    /// </summary>
    /// <param name="docs">A list of docs to upload</param>
    Task UploadDocumentsAsync(List<SearchIndexDocument> docs);

}