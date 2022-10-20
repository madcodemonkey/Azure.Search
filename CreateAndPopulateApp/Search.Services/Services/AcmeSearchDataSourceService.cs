using Azure;
using Azure.Search.Documents.Indexes.Models;

namespace Search.Services;

public class AcmeSearchDataSourceService : IAcmeSearchDataSourceService
{
    private readonly SearchServiceSettings _settings;
    private readonly IAcmeSearchIndexerService _indexerService;

    /// <summary>Constructor</summary>
    public AcmeSearchDataSourceService(SearchServiceSettings settings, IAcmeSearchIndexerService indexerService)
    {
        _settings = settings;
        _indexerService = indexerService;
    }

    /// <summary>Creates a Azure SQL datasource that will be used by an indexer.</summary>
    /// <param name="dataSourceConnectionName">The name of the data source</param>
    /// <param name="tableOrViewName">The table or view that the Azure SQL datasource is pointed at.</param>
    public async Task CreateAzureSqlDataSourceAsync(string dataSourceConnectionName, string tableOrViewName)
    {
        var dataSource = new SearchIndexerDataSourceConnection(
            dataSourceConnectionName, SearchIndexerDataSourceType.AzureSql,
            _settings.DatabaseConnectionString,
            new SearchIndexerDataContainer(tableOrViewName));

        // The data source does not need to be deleted if it was already created,
        // but the connection string may need to be updated if it was changed
        await _indexerService.ClientIndexer.CreateOrUpdateDataSourceConnectionAsync(dataSource);
    }

    /// <summary>Gets a list of data sources</summary>
    /// <param name="dataSourceConnectionName">The name of the data source</param>
    public async Task<bool> DeleteDataSourceAsync(string dataSourceConnectionName)
    {
        Response<SearchIndexerDataSourceConnection>? dataSource = await _indexerService.ClientIndexer.GetDataSourceConnectionAsync(dataSourceConnectionName);

        if (dataSource != null)
        {
            await _indexerService.ClientIndexer.DeleteDataSourceConnectionAsync(dataSource);
            return true;
        }

        return false;
    }


    /// <summary>Gets a list of data sources</summary>
    public async Task<List<string>> GetDataSourceListAsync()
    {
        Response<IReadOnlyList<string>> response = await _indexerService.ClientIndexer.GetDataSourceConnectionNamesAsync();

        List<string> result = response.Value.ToList();

        return result;
    }

}