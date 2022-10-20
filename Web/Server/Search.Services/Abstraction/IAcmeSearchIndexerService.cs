using Azure.Search.Documents.Indexes;

namespace Search.Services;

public interface IAcmeSearchIndexerService
{
    SearchIndexerClient ClientIndexer { get; }

    /// <summary>Gets a list of data sources</summary>
    /// <param name="indexerName">The name of the indexer</param>
    Task<bool> DeleteAsync(string indexerName);

    /// <summary>Gets a list of indexers</summary>
    Task<List<string>> GetListAsync();

    /// <summary>Runs an indexer now.</summary>
    /// <param name="indexerName">Name of the indexer to run</param>
    Task RunAsync(string indexerName);
}