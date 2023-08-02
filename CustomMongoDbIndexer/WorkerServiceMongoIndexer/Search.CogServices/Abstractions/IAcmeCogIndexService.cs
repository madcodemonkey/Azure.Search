using Azure.Search.Documents.Models;
using Azure;

namespace Search.CogServices;

public interface IAcmeCogIndexService
{
    /// <summary>Deletes an index.</summary>
    /// <param name="indexName">The name of the index to delete</param>
    Task<bool> DeleteAsync(string indexName);

    /// <summary>Indicates if an index exists.</summary>
    /// <param name="indexName">The name of the index to find.</param>
    /// <remarks>Unfortunately, we get this response as an exception from the API,
    /// so we have to check for the HTTP status code of 404 to determine if it was really missing or if there was
    /// some type of other error</remarks>
    Task<bool> ExistsAsync(string indexName);

    /// <summary>Returns a list of index names.</summary>
    Task<List<string>> GetIndexNamesAsync();
    
    /// <summary>
    /// Clears all documents from the index.
    /// </summary>
    /// <param name="indexName">The name of the index</param>
    /// <param name="keyField">The name of the key field that uniquely identifies documents in the index.</param>
    /// <returns>The number of documents deleted</returns>
    Task<long> DeleteAllDocumentsAsync(string indexName, string keyField);

    /// <summary>
    /// Clears the specified documents from the index.
    /// </summary>
    /// <param name="indexName">The name of the index</param>
    /// <param name="keyField">The name of the key field that uniquely identifies documents in the index.</param>
    /// <param name="key">The key (primary field) of the document to delete.</param>
    /// <returns>The number of documents deleted</returns>
    Task<bool> DeleteDocumentAsync(string indexName, string keyField, string key);

    /// <summary>
    /// Clears the specified documents from the index.
    /// </summary>
    /// <param name="indexName">The name of the index</param>
    /// <param name="keyField">The name of the key field that uniquely identifies documents in the index.</param>
    /// <param name="keys">The keys of the documents to delete.</param>
    /// <returns>The number of documents deleted</returns>
    Task<long> DeleteDocumentsAsync(string indexName, string keyField, List<string> keys);

    /// <summary>Retrieves a single document.</summary>
    /// <typeparam name="T">The type of data being returned.</typeparam>
    /// <param name="indexName">The name of the index</param>
    /// <param name="key">The documents key</param>
    Task<Response<T>?> GetDocumentAsync<T>(string indexName, string key);

    /// <summary>
    /// Uploads one document to an index.
    /// </summary>
    /// <param name="indexName">The name of the index</param>
    /// <param name="doc">One document</param>
    Task<IndexDocumentsResult> UploadDocumentAsync<T>(string indexName, T doc);

    /// <summary>Uploads documents to an index.</summary>
    /// <typeparam name="T">The class type that we are uploading.</typeparam>
    /// <param name="indexName">The name of the index</param>
    /// <param name="documentList">The list of items of type T to upload.</param>
    Task<IndexDocumentsResult> UploadDocuments<T>(string indexName, List<T> documentList);
}