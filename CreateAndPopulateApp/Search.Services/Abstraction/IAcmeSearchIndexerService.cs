using Azure.Search.Documents.Indexes;

namespace Search.Services;

public interface IAcmeSearchIndexerService
{
    SearchIndexerClient ClientIndexer { get; }
    Task RunAsync(string indexerName);

    /// <summary>Gets a list of data sources</summary>
    /// <param name="indexerName">The name of the indexer</param>
    Task<bool> DeleteAsync(string indexerName);

    /// <summary>Gets a list of indexers</summary>
    Task<List<string>> GetListAsync();
}