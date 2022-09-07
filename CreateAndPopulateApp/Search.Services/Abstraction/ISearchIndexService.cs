using Azure.Search.Documents.Indexes;

namespace Search.Services;

public interface ISearchIndexService
{
    SearchIndexClient Client { get; }
    Task<bool> CreateIndexAsync<T>(T typeToCreate, string indexName);
    Task<bool> DeleteIndexAsync(string indexName);
}