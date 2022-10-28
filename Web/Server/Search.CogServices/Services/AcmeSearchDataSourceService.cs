using Azure;
using Azure.Search.Documents.Indexes.Models;

namespace Search.CogServices;

public class AcmeSearchDataSourceService : IAcmeSearchDataSourceService
{
    protected IAcmeSearchIndexerService IndexerService { get; }

    /// <summary>Constructor</summary>
    public AcmeSearchDataSourceService(IAcmeSearchIndexerService indexerService)
    {
        IndexerService = indexerService;
    }

    /// <summary>Creates a Azure SQL data source that will be used by an indexer.</summary>
    /// <param name="name">The name of the data source</param>
    /// <param name="tableOrViewName">The table or view that the Azure SQL data source is pointed at.</param>
    /// <param name="connectionString">A connection string to attach to the database.</param>
    public async Task<bool> CreateForAzureSqlAsync(string name, string tableOrViewName, string connectionString)
    {
        var dataSource = new SearchIndexerDataSourceConnection(
            name, SearchIndexerDataSourceType.AzureSql, connectionString,
            new SearchIndexerDataContainer(tableOrViewName));

        // The data source does not need to be deleted if it was already created,
        // but the connection string may need to be updated if it was changed
        var results = await IndexerService.ClientIndexer.CreateOrUpdateDataSourceConnectionAsync(dataSource);

        return results != null;  // Is this a good check?
    }

    /// <summary>Gets a list of data sources</summary>
    /// <param name="dataSourceConnectionName">The name of the data source</param>
    public async Task<bool> ExistsAsync(string dataSourceConnectionName)
    {
        Response<SearchIndexerDataSourceConnection>? dataSource = await IndexerService.ClientIndexer.GetDataSourceConnectionAsync(dataSourceConnectionName);

        return dataSource != null;
    }

    /// <summary>Gets a list of data sources</summary>
    /// <param name="dataSourceConnectionName">The name of the data source</param>
    public async Task<bool> DeleteAsync(string dataSourceConnectionName)
    {
        Response<SearchIndexerDataSourceConnection>? dataSource = await IndexerService.ClientIndexer.GetDataSourceConnectionAsync(dataSourceConnectionName);

        if (dataSource != null)
        {
            await IndexerService.ClientIndexer.DeleteDataSourceConnectionAsync(dataSource);
            return true;
        }

        return false;
    }
    
    /// <summary>Gets a list of data sources</summary>
    public async Task<List<string>> GetListAsync()
    {
        Response<IReadOnlyList<string>> response = await IndexerService.ClientIndexer.GetDataSourceConnectionNamesAsync();

        List<string> result = response.Value.ToList();

        return result;
    }

}