using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using CogSearchServices.Models;

namespace CogSearchServices.Services;

public interface ICogSearchIndexService
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
    /// Clears the specified documents from the index.
    /// </summary>
    /// <param name="keyField">The name of the key field that uniquely identifies documents in the index.</param>
    /// <param name="key">The key (primary field) of the document to delete.</param>
    /// <returns>The number of documents deleted</returns>
    Task<bool> DeleteDocumentAsync(string keyField, string key);

    /// <summary>Searches for documents</summary>
    /// <typeparam name="T">The type of data being returned.</typeparam>
    /// <param name="searchText">The text to find</param>
    /// <param name="options">The search options to apply</param>
    Task<CogSearchQueryResponse<T>> SearchAsync<T>(string searchText, SearchOptions options) where T : class;

    /// <summary>
    /// Uploads one document to an index.
    /// </summary>
    /// <param name="doc">One document</param>
    Task<IndexDocumentsResult> UploadDocumentAsync<T>(T doc) where T : class;

    /// <summary>Uploads documents to an index.</summary>
    /// <typeparam name="T">The class type that we are uploading.</typeparam>
    /// <param name="documentList">The list of items of type T to upload.</param>
    Task<IndexDocumentsResult> UploadDocumentsAsync<T>(List<T> documentList) where T : class;
}