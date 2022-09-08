using ConsoleMenuHelper;
using Microsoft.Extensions.Configuration;
using Search.Services;

namespace CreateAndPopulateApp;

[ConsoleMenuItem("AzureSql", 2)]
public class AzureSqlDataSourceCreateMenuItem : IConsoleMenuItem
{
    private readonly IPromptHelper _promptHelper;
    private readonly ISearchDataSourceService _dataSourceService;
    private readonly string _defaultDataSourceName;
    private readonly string _defaultTableName;

    /// <summary>Constructor</summary>
    public AzureSqlDataSourceCreateMenuItem(IConfiguration configuration, IPromptHelper promptHelper, ISearchDataSourceService dataSourceService)
    {
        _promptHelper = promptHelper;
        _dataSourceService = dataSourceService;
        
        _defaultTableName = configuration["SearchServiceAzureSqlTableName"];
        _defaultDataSourceName = configuration["SearchServiceAzureSqlDataSourceName"];
    }

    public async Task<ConsoleMenuItemResponse> WorkAsync()
    {
        string dataSourceName = _promptHelper.GetText($"Name of the HOTEL Data Source (Default: {_defaultDataSourceName})?", true, true);
        if (string.IsNullOrWhiteSpace(dataSourceName))
            dataSourceName = _defaultDataSourceName;

        if (dataSourceName == "exit")
            return new ConsoleMenuItemResponse(false, false);

        string tableName = _promptHelper.GetText($"Name of the Index (Default: {_defaultTableName})?", true, true);
        if (string.IsNullOrWhiteSpace(tableName))
            tableName = _defaultTableName;

        if (tableName == "exit")
            return new ConsoleMenuItemResponse(false, false);

        Console.WriteLine("Creating the data source that is needed for the indexer...");
        await _dataSourceService.CreateAzureSqlDataSourceAsync(dataSourceName, tableName);

        Console.WriteLine("-------------------------------");

        return new ConsoleMenuItemResponse(false, false);
    }

    public string ItemText => "Data Source Create  (needed before Indexer is created)";

    /// <summary>Optional data from the attribute.</summary>
    public string AttributeData { get; set; } = string.Empty;
}