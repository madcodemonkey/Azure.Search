﻿namespace Search.Services;

public interface ISearchDataSourceService
{
    /// <summary>Creates a Azure SQL data source that will be used by an indexer.</summary>
    /// <param name="name">The name of the data source</param>
    /// <param name="tableOrViewName">The table or view that the Azure SQL data source is pointed at.</param>
    /// <param name="connectionString">A connection string to attach to the database.</param>
    Task<bool> CreateForAzureSqlAsync(string name, string tableOrViewName, string connectionString);

    /// <summary>Gets a list of data sources</summary>
    Task<List<string>> GetListAsync();

    /// <summary>Gets a list of data sources</summary>
    /// <param name="dataSourceConnectionName">The name of the data source</param>
    Task<bool> DeleteAsync(string dataSourceConnectionName);

    /// <summary>Gets a list of data sources</summary>
    /// <param name="dataSourceConnectionName">The name of the data source</param>
    Task<bool> ExistsAsync(string dataSourceConnectionName);
}