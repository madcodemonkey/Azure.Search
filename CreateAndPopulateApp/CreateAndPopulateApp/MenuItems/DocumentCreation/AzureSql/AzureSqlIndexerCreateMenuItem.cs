using ConsoleMenuHelper;
using Search.CogServices;
using Search.Services;

namespace CreateAndPopulateApp;

[ConsoleMenuItem("AzureSql", 6)]
public class AzureSqlIndexerCreateMenuItem : IConsoleMenuItem
{
    private readonly SearchServiceSettings _settings;
    private readonly IPromptHelper _promptHelper;
    private readonly IAcmeSearchIndexerService _indexerService;
    private readonly IHotelIndexerService _hotelIndexerService;

    /// <summary>Constructor</summary>
    public AzureSqlIndexerCreateMenuItem(SearchServiceSettings settings, IPromptHelper promptHelper, IAcmeSearchIndexerService indexerService,
        IHotelIndexerService hotelIndexerService)
    {
        _settings = settings;
        _promptHelper = promptHelper;
        _indexerService = indexerService;
        _hotelIndexerService = hotelIndexerService;
    }

    public async Task<ConsoleMenuItemResponse> WorkAsync()
    {
        
        string dataSourceName = _promptHelper.GetText($"Name of the HOTEL Data Source (Default: {_settings.HotelDataSourceName})?", true, true);
        if (string.IsNullOrWhiteSpace(dataSourceName))
            dataSourceName = _settings.HotelDataSourceName;

        if (dataSourceName == "exit")
            return new ConsoleMenuItemResponse(false, false);
        
        string indexName = _promptHelper.GetText($"Name of the Index (Default: {_settings.HotelIndexName})?", true, true);
        if (string.IsNullOrWhiteSpace(indexName))
            indexName = _settings.HotelIndexName;

        if (indexName == "exit")
            return new ConsoleMenuItemResponse(false, false);
        
        string indexerName = _promptHelper.GetText($"Name of the HOTEL Indexer (Default: {_settings.HotelIndexerName})?", true, true);
        if (string.IsNullOrWhiteSpace(indexerName))
            indexerName = _settings.HotelIndexerName;

        if (indexerName == "exit")
            return new ConsoleMenuItemResponse(false, false);

        Console.WriteLine("Creating the indexer that will use the data source to pull info from the Hotel table...");
        await _hotelIndexerService.CreateIndexerAsync(indexerName, dataSourceName, indexName);

        if (_promptHelper.GetYorN("Do you want to run the indexer now?", true))
        {
            Console.WriteLine("Running the index now....");
            await _indexerService.RunAsync(indexerName);
        }

        Console.WriteLine("-------------------------------");

        return new ConsoleMenuItemResponse(false, false);
    }

    public string ItemText => "Azure SQL Create Hotels Indexer";

    /// <summary>Optional data from the attribute.</summary>
    public string AttributeData { get; set; } = string.Empty;


}