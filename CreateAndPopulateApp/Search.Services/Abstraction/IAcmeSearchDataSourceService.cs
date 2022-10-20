namespace Search.Services;

public interface IAcmeSearchDataSourceService
{
    /// <summary>Creates a Azure SQL datasource that will be used by an indexer.</summary>
    /// <param name="name">The name of the data source</param>
    /// <param name="tableOrViewName">The table or view that the Azure SQL datasource is pointed at.</param>
    /// <param name="connectionString">A connection string to attach to the database.</param>
    Task<bool> CreateForAzureSqlAsync(string name, string tableOrViewName, string connectionString);

    /// <summary>Gets a list of data sources</summary>
    /// <param name="dataSourceConnectionName">The name of the data source</param>
    Task<bool> ExistsAsync(string dataSourceConnectionName);

    /// <summary>Gets a list of data sources</summary>
    Task<List<string>> GetListAsync();

    /// <summary>Gets a list of data sources</summary>
    /// <param name="dataSourceConnectionName">The name of the data source</param>
    Task<bool> DeleteAsync(string dataSourceConnectionName);
}