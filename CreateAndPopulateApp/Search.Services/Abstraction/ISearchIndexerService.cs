using Azure.Search.Documents.Indexes;

namespace Search.Services;

public interface ISearchIndexerService
{
    SearchIndexerClient ClientIndexer { get; }
    Task CreateAzureSqlDataSourceAsync(string dataSourceName, string tableOrViewName);
    Task RunIndexerAsync(string indexerName);
}