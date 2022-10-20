using ConsoleMenuHelper;
using Search.Services;

namespace CreateAndPopulateApp;

[ConsoleMenuItem("AzureSql", 3)]
public class AzureSqlDataSourceDeleteMenuItem : IConsoleMenuItem
{
    private readonly SearchServiceSettings _settings;
    private readonly IPromptHelper _promptHelper;
    private readonly IAcmeSearchDataSourceService _dataSourceService;

    /// <summary>Constructor</summary>
    public AzureSqlDataSourceDeleteMenuItem(SearchServiceSettings settings, IPromptHelper promptHelper, IAcmeSearchDataSourceService dataSourceService)
    {
        _settings = settings;
        _promptHelper = promptHelper;
        _dataSourceService = dataSourceService;
    }

    public async Task<ConsoleMenuItemResponse> WorkAsync()
    {
        string dataSourceName = _promptHelper.GetText($"Name of the Data Source to delete (Default: {_settings.HotelDataSourceName})?", true, true);
        if (string.IsNullOrWhiteSpace(dataSourceName))
            dataSourceName = _settings.HotelDataSourceName;

        if (dataSourceName == "exit")
            return new ConsoleMenuItemResponse(false, false);

        var result = await _dataSourceService.DeleteAsync(dataSourceName);
        if (result)
            Console.WriteLine($"Deleted {dataSourceName}");
        else Console.WriteLine("NOTHING deleted");

        Console.WriteLine("-------------------------------");

        return new ConsoleMenuItemResponse(false, false);
    }

    public string ItemText => "Data Source delete";

    /// <summary>Optional data from the attribute.</summary>
    public string AttributeData { get; set; } = string.Empty;
}