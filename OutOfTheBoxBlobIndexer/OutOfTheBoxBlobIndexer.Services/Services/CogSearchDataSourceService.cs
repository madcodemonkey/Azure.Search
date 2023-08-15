using Azure;
using Azure.Search.Documents.Indexes.Models;
using OutOfTheBoxBlobIndexer.Models;

namespace OutOfTheBoxBlobIndexer.Services;

public class CogSearchDataSourceService : ICogSearchDataSourceService
{
    private readonly ICogClientWrapperService _clientService;

    /// <summary>
    /// Constructor
    /// </summary>
    public CogSearchDataSourceService(ICogClientWrapperService clientService)
    {
        _clientService = clientService;
    }
    
    /// <summary>Creates a Azure SQL data source that will be used by an indexer.</summary>
    /// <param name="name">The name of the data source</param>
    /// <param name="tableOrViewName">The table or view that the Azure SQL data source is pointed at.</param>
    /// <param name="connectionString">A connection string to attach to the database.</param>
    /// <param name="highWaterMarkColumnName">The high watermark field used to detect changes</param>
    /// <param name="softDeleteColumnName">The column that indicates that the record should be removed from the Azure Search Index</param>
    /// <param name="softDeleteColumnValue">The value in the column that indicates that the record should be deleted.</param>
    public async Task<bool> CreateForAzureSqlAsync(string name, string tableOrViewName, string connectionString,
        string highWaterMarkColumnName, string softDeleteColumnName, string softDeleteColumnValue)
    {
        var dataSource = new SearchIndexerDataSourceConnection(
            name, SearchIndexerDataSourceType.AzureSql, connectionString,
            new SearchIndexerDataContainer(tableOrViewName))
        {
            DataChangeDetectionPolicy = new HighWaterMarkChangeDetectionPolicy(highWaterMarkColumnName),
            DataDeletionDetectionPolicy = new SoftDeleteColumnDeletionDetectionPolicy
            {
                SoftDeleteColumnName = softDeleteColumnName,
                SoftDeleteMarkerValue = softDeleteColumnValue
            }
        };

        // The data source does not need to be deleted if it was already created,
        // but the connection string may need to be updated if it was changed
        var indexerClient = _clientService.GetIndexerClient();
        var results = await indexerClient.CreateOrUpdateDataSourceConnectionAsync(dataSource);

        return results != null;  // Is this a good check?
    }

    /// <summary>Creates a blob storage data source that will be used an out-of-the-box indexer.</summary>
    /// <param name="name">The name of the data source</param>
    /// <param name="containerName">The name of the container within the Azure storage resource.</param>
    /// <param name="connectionString">A connection string to attach to the Azure storage resource.</param>
    /// <param name="softDeleteColumnName">The column that indicates that the record should be removed from the Azure Search Index</param>
    /// <param name="softDeleteColumnValue">The value in the column that indicates that the record should be deleted.</param>
    public async Task<bool> CreateForBlobAsync(string name, string containerName, string connectionString,
        string softDeleteColumnName, string softDeleteColumnValue, CancellationToken cancellationToken = default)
    {
        var dataSource = new SearchIndexerDataSourceConnection(
            name, SearchIndexerDataSourceType.AzureBlob, connectionString,
            new SearchIndexerDataContainer(containerName))
        {
            DataDeletionDetectionPolicy = new SoftDeleteColumnDeletionDetectionPolicy
            {
                SoftDeleteColumnName = softDeleteColumnName,
                SoftDeleteMarkerValue = softDeleteColumnValue
            }
        };

        // The data source does not need to be deleted if it was already created,
        // but the connection string may need to be updated if it was changed
        var indexerClient = _clientService.GetIndexerClient();
        var results = await indexerClient.CreateOrUpdateDataSourceConnectionAsync(dataSource, cancellationToken: cancellationToken);

        return results != null;  // Is this a good check?
    }

    /// <summary>Creates a Cosmos DB Apache Gremlin data source that will be used by an indexer.</summary>
    /// <param name="name">The name of the data source</param>
    /// <param name="containerName">The container that houses the documents</param>
    /// <param name="connectionString">A connection string to attach to Cosmos DB Apache Gremlin.</param>
    /// <param name="queryType">The type of query to perform (vertices or edges)</param>
    /// <param name="softDeleteColumnName">The column that indicates that the record should be removed from the Azure Search Index</param>
    /// <param name="softDeleteColumnValue">The value in the column that indicates that the record should be deleted.</param>
    /// <remarks>
    /// See docs: https://learn.microsoft.com/en-us/azure/search/search-howto-index-cosmosdb-gremlin#define-the-data-source
    /// </remarks>
    public async Task<bool> CreateForGremlinAsync(string name, string containerName, string connectionString,
        GremlinQueryTypes queryType,  string? softDeleteColumnName, string? softDeleteColumnValue)
    {
        var searchIndexerDataContainer = new SearchIndexerDataContainer(containerName)
        {
            Query = queryType == GremlinQueryTypes.Edges ? "g.E()" : "g.V()"
        }; 

        var dataSource = new SearchIndexerDataSourceConnection(name, 
            SearchIndexerDataSourceType.CosmosDb, 
            connectionString,
            searchIndexerDataContainer )
        {
            DataChangeDetectionPolicy = new HighWaterMarkChangeDetectionPolicy("_ts"),
        };

        if (!string.IsNullOrWhiteSpace(softDeleteColumnName) && !string.IsNullOrWhiteSpace(softDeleteColumnValue))
        {
            dataSource.DataDeletionDetectionPolicy = new SoftDeleteColumnDeletionDetectionPolicy
            {
                SoftDeleteColumnName = softDeleteColumnName,
                SoftDeleteMarkerValue = softDeleteColumnValue
            };
        }
        

        // The data source does not need to be deleted if it was already created,
        // but the connection string may need to be updated if it was changed
        var indexerClient = _clientService.GetIndexerClient();
        var results = await indexerClient.CreateOrUpdateDataSourceConnectionAsync(dataSource);

        return results != null;  // Is this a good check?
    }


    /// <summary>Delete a data sources</summary>
    /// <param name="dataSourceConnectionName">The name of the data source</param>
    public async Task<bool> DeleteAsync(string dataSourceConnectionName)
    {
        var indexerClient = _clientService.GetIndexerClient();

        Response<SearchIndexerDataSourceConnection>? dataSource = await indexerClient.GetDataSourceConnectionAsync(dataSourceConnectionName);

        if (dataSource != null)
        {
            await indexerClient.DeleteDataSourceConnectionAsync(dataSource);
            return true;
        }

        return false;
    }

    /// <summary>Gets a list of data sources</summary>
    /// <param name="dataSourceConnectionName">The name of the data source</param>
    public async Task<bool> ExistsAsync(string dataSourceConnectionName)
    {
        var indexerClient = _clientService.GetIndexerClient();

        Response<SearchIndexerDataSourceConnection>? dataSource = await indexerClient.GetDataSourceConnectionAsync(dataSourceConnectionName);

        return dataSource != null;
    }

    /// <summary>Gets a list of data sources</summary>
    public async Task<List<string>> GetListAsync()
    {
        var indexerClient = _clientService.GetIndexerClient();

        Response<IReadOnlyList<string>> response = await indexerClient.GetDataSourceConnectionNamesAsync();

        List<string> result = response.Value.ToList();

        return result;
    }
}