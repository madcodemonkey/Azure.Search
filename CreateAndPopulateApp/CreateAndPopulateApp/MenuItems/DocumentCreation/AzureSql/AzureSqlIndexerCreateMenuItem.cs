using ConsoleMenuHelper;
using Search.Services;

namespace CreateAndPopulateApp;

[ConsoleMenuItem("AzureSql", 6)]
public class AzureSqlIndexerCreateMenuItem : IConsoleMenuItem
{
    private readonly SearchServiceSettings _settings;
    private readonly IPromptHelper _promptHelper;
    private readonly IHotelIndexerService _hotelIndexerService;

    /// <summary>Constructor</summary>
    public AzureSqlIndexerCreateMenuItem(SearchServiceSettings settings, IPromptHelper promptHelper, IHotelIndexerService hotelIndexerService)
    {
        _settings = settings;
        _promptHelper = promptHelper;
        _hotelIndexerService = hotelIndexerService;
    }

    public async Task<ConsoleMenuItemResponse> WorkAsync()
    {
        Console.WriteLine($"Name of the HOTEL Data Source: {_settings.Hotel.DataSourceName}");
        Console.WriteLine($"Name of the Index: {_settings.Hotel.IndexName}");
        Console.WriteLine($"Name of the HOTEL Indexer (Default: {_settings.Hotel.IndexerName}");
        Console.WriteLine("  ");

        if (_promptHelper.GetYorN("Create indexer?", true))
        {
            Console.WriteLine("Creating the indexer that will use the data source to pull info from the Hotel table...");
            await _hotelIndexerService.CreateAsync();

            if (_promptHelper.GetYorN("Do you want to run the indexer now?", true))
            {
                Console.WriteLine("Running the index now....");
                await _hotelIndexerService.RunAsync();
            }
        }

        Console.WriteLine("-------------------------------");

        return new ConsoleMenuItemResponse(false, false);
    }

    public string ItemText => "Azure SQL Create Hotels Indexer";

    /// <summary>Optional data from the attribute.</summary>
    public string AttributeData { get; set; } = string.Empty;


}