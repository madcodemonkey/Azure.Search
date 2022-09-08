using ConsoleMenuHelper;
using Microsoft.Extensions.Configuration;
using Search.Services;

namespace CreateAndPopulateApp;

[ConsoleMenuItem("AzureSql", 3)]
public class AzureSqlDataSourceDeleteMenuItem : IConsoleMenuItem
{
    private readonly IPromptHelper _promptHelper;
    private readonly ISearchDataSourceService _dataSourceService;
    private readonly string _defaultDataSourceName;

    /// <summary>Constructor</summary>
    public AzureSqlDataSourceDeleteMenuItem(IConfiguration configuration, IPromptHelper promptHelper, ISearchDataSourceService dataSourceService)
    {
        _promptHelper = promptHelper;
        _dataSourceService = dataSourceService;
     
        _defaultDataSourceName = configuration["SearchServiceAzureSqlDataSourceName"];
    }

    public async Task<ConsoleMenuItemResponse> WorkAsync()
    {
        string dataSourceName = _promptHelper.GetText($"Name of the Data Source to delete (Default: {_defaultDataSourceName})?", true, true);
        if (string.IsNullOrWhiteSpace(dataSourceName))
            dataSourceName = _defaultDataSourceName;

        if (dataSourceName == "exit")
            return new ConsoleMenuItemResponse(false, false);
        
        var result = await _dataSourceService.DeleteDataSourceAsync(dataSourceName);
        if (result)
            Console.WriteLine($"Deleted {dataSourceName}");
        else  Console.WriteLine("NOTHING deleted");

        Console.WriteLine("-------------------------------");

        return new ConsoleMenuItemResponse(false, false);
    }

    public string ItemText => "Data Source delete";

    /// <summary>Optional data from the attribute.</summary>
    public string AttributeData { get; set; } = string.Empty;
}