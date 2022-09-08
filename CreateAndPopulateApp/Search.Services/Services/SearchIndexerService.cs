using Azure;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;

namespace Search.Services;

public class SearchIndexerService : ISearchIndexerService
{
    private readonly SearchServiceSettings _settings;
    private SearchIndexerClient? _client;
    //  private readonly SearchClientOptions _clientOptions;

    /// <summary>Constructor</summary>
    public SearchIndexerService(SearchServiceSettings settings)
    {
        _settings = settings;
        //    _clientOptions =  CreateSearchClientOptions();
    }

    public SearchIndexerClient ClientIndexer => _client ??= new SearchIndexerClient(
        new Uri(_settings.SearchEndPoint), new AzureKeyCredential(_settings.SearchAdminApiKey));


    public async Task CreateAzureSqlDataSourceAsync(string dataSourceName, string tableOrViewName)
    {
        var dataSource = new SearchIndexerDataSourceConnection(
            dataSourceName, SearchIndexerDataSourceType.AzureSql,
            _settings.SearchAzureSqlConnectionString,
            new SearchIndexerDataContainer(tableOrViewName));

        // The data source does not need to be deleted if it was already created,
        // but the connection string may need to be updated if it was changed
        await ClientIndexer.CreateOrUpdateDataSourceConnectionAsync(dataSource);
    }

    public async Task RunIndexerAsync(string indexerName)
    {
        await ClientIndexer.RunIndexerAsync(indexerName);
    }
}