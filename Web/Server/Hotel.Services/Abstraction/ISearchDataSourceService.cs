namespace Hotel.Services;

public interface ISearchDataSourceService
{
    Task CreateAzureSqlDataSourceAsync(string dataSourceConnectionName, string tableOrViewName);

    /// <summary>Gets a list of data sources</summary>
    Task<List<string>> GetDataSourceListAsync();

    /// <summary>Gets a list of data sources</summary>
    /// <param name="dataSourceConnectionName">The name of the data source</param>
    Task<bool> DeleteDataSourceAsync(string dataSourceConnectionName);
}