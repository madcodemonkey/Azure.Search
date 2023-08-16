using Azure.Search.Documents;
using CogSimple.Services;
using CustomSqlServerIndexer.Models;

namespace CustomSqlServerIndexer.Services;

public interface ICustomSearchIndexService
{
    /// <summary>
    /// Clears all documents from the index.
    /// </summary>
    /// <param name="keyField"></param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>The number of documents deleted</returns>
    Task<long> DeleteAllDocumentsAsync(string keyField, CancellationToken cancellationToken);

    /// <summary>
    /// Clears the specified documents from the index.
    /// </summary>
    /// <param name="keyField">The name of the key field that uniquely identifies documents in the index.</param>
    /// <param name="keys">The keys of the documents to delete.</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>The number of documents deleted</returns>
    Task<long> DeleteDocumentsAsync(string keyField, List<string> keys, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes the entire index and all it's documents!
    /// </summary>
    /// <param name="cancellationToken">A cancellation token</param>
    Task DeleteIndexAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Create index or update the index.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token</param>
    Task CreateOrUpdateIndexAsync(CancellationToken cancellationToken);

    /// <summary>Searches for documents</summary>
    /// <typeparam name="T">The type of data being returned.</typeparam>
    /// <param name="searchText">The text to find</param>
    /// <param name="options">The search options to apply</param>
    /// <param name="cancellationToken">A cancellation token</param>
    Task<SearchQueryResponse<T>> SearchAsync<T>(string searchText, SearchOptions options, CancellationToken cancellationToken) where T : class;


    /// <summary>
    /// Upload multiple documents in a single Upload request.
    /// </summary>
    /// <param name="docs">A list of docs to upload</param>
    /// <param name="cancellationToken">A cancellation token</param>
    Task UploadDocumentsAsync(List<SearchIndexDocument> docs, CancellationToken cancellationToken);

}