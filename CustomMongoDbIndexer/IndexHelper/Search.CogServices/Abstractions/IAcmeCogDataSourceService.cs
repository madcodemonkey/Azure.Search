namespace Search.CogServices;

public interface IAcmeCogDataSourceService
{
    /// <summary>Creates a Azure SQL data source that will be used by an indexer.</summary>
    /// <param name="name">The name of the data source</param>
    /// <param name="tableOrViewName">The table or view that the Azure SQL data source is pointed at.</param>
    /// <param name="connectionString">A connection string to attach to the database.</param>
    /// <param name="highWaterMarkColumnName">The high watermark field used to detect changes</param>
    /// <param name="softDeleteColumnName">The column that indicates that the record should be removed from the Azure Search Index</param>
    /// <param name="softDeleteColumnValue">The value in the column that indicates that the record should be deleted.</param>
    Task<bool> CreateForAzureSqlAsync(string name, string tableOrViewName, string connectionString, string highWaterMarkColumnName, string softDeleteColumnName, string softDeleteColumnValue);

    /// <summary>Delete a data sources</summary>
    /// <param name="dataSourceConnectionName">The name of the data source</param>
    Task<bool> DeleteAsync(string dataSourceConnectionName);

    /// <summary>Gets a list of data sources</summary>
    /// <param name="dataSourceConnectionName">The name of the data source</param>
    Task<bool> ExistsAsync(string dataSourceConnectionName);

    /// <summary>Gets a list of data sources</summary>
    Task<List<string>> GetListAsync();
}