using Azure.Search.Documents.Indexes;

namespace Search.Services;

public interface ISearchIndexService
{
    SearchIndexClient Client { get; }
    Task<bool> CreateOrUpdateAsync(Type typeToCreate, string indexName);
    Task<bool> DeleteAsync(string indexName);

    /// <summary>Returns a list of index names.</summary>
    Task<List<string>> GetIndexNamesAsync();

    /// <summary>Uploads documents to an index.</summary>
    /// <typeparam name="T">The class type that we are uploading.</typeparam>
    /// <param name="indexName">The name of the index</param>
    /// <param name="uploadList">The list of items of type T to upload.</param>
    Task UploadDocuments<T>(string indexName, List<T> uploadList);
}