using Azure.Search.Documents;
using CustomBlobIndexer.Models;

namespace CustomBlobIndexer.Services;

public interface ICustomSearchIndexService
{
    /// <summary>
    /// Clears all documents from the index.
    /// </summary>
    Task<long> ClearAllDocumentsAsync();

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