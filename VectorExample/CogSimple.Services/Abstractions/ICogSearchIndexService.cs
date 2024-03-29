﻿using Azure.Search.Documents;

namespace CogSimple.Services;

public interface ICogSearchIndexService
{
    /// <summary>
    /// Clears all documents from the index.
    /// </summary>
    /// <param name="indexName">The name of the index.</param>
    /// <param name="keyField">The name of the key field that uniquely identifies documents in the index.</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>The number of documents deleted</returns>
    Task<long> DeleteAllDocumentsAsync(string indexName, string keyField, CancellationToken cancellationToken = default);

    /// <summary>
    /// Clears the specified documents from the index.
    /// </summary>
    /// <param name="indexName">The name of the index.</param>
    /// <param name="keyField">The name of the key field that uniquely identifies documents in the index.</param>
    /// <param name="keys">The keys of the documents to delete.</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>The number of documents deleted</returns>
    Task<long> DeleteDocumentsAsync(string indexName, string keyField, List<string> keys, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes the entire index and all it's documents!
    /// </summary>
    /// <param name="indexName">The name of the index.</param>
    /// <param name="checkIfExistsFirst">Indicates if you want the code to check to make sure the indexer exists before attempting to delete it.  If you try
    /// to delete an indexer that doesn't exist, it will generate an exception.</param>  /// <param name="cancellationToken">A cancellation token</param>
    /// <returns></returns>
    Task DeleteIndexAsync(string indexName, bool checkIfExistsFirst, CancellationToken cancellationToken = default);

    /// <summary>
    /// Counts the number of documents inside the index.
    /// </summary>
    /// <param name="indexName">The name of the index</param>
    /// <param name="cancellationToken">A cancellation token</param>
    Task<long> DocumentCountAsync(string indexName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Indicates if an index exists or not
    /// </summary>
    /// <param name="indexName">The name of the index</param>
    /// <param name="cancellationToken">A cancellation token</param>
    Task<bool> IndexExistsAsync(string indexName, CancellationToken cancellationToken = default);

    /// <summary>Searches for documents</summary>
    /// <typeparam name="T">The type of data being returned.</typeparam>
    /// <param name="indexName">The name of the index.</param>
    /// <param name="searchText">The text to find</param>
    /// <param name="options">The search options to apply</param>
    /// <param name="cancellationToken">A cancellation token</param>
    Task<SearchQueryResponse<T>> SearchAsync<T>(string indexName, string? searchText, SearchOptions options, CancellationToken cancellationToken = default) where T : class;

    /// <summary>
    /// Upload one document in a single Upload request.
    /// </summary>
    /// <param name="indexName">The name of the index.</param>
    /// <param name="doc">One document to upload</param>
    /// <param name="cancellationToken">A cancellation token</param>
    Task UploadDocumentsAsync<T>(string indexName, T doc, CancellationToken cancellationToken = default) where T : class;

    /// <summary>
    /// Upload multiple documents in a single Upload request.
    /// </summary>
    /// <param name="indexName">The name of the index.</param>
    /// <param name="docs">A list of docs to upload</param>
    /// <param name="cancellationToken">A cancellation token</param>
    Task UploadDocumentsAsync<T>(string indexName, List<T> docs, CancellationToken cancellationToken = default) where T: class ;
}