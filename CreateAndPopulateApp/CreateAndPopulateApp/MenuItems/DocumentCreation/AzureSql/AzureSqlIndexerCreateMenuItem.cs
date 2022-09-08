using ConsoleMenuHelper;
using Microsoft.Extensions.Configuration;
using Search.Services;

namespace CreateAndPopulateApp;

[ConsoleMenuItem("AzureSql", 6)]
public class AzureSqlIndexerCreateMenuItem : IConsoleMenuItem
{
    private readonly IPromptHelper _promptHelper;
    private readonly ISearchIndexerService _indexerService;
    private readonly IHotelIndexerService _hotelIndexerService;
    private readonly string _defaultIndexName;
    private readonly string _defaultIndexerName;
    private readonly string _defaultDataSourceName;
 
    /// <summary>Constructor</summary>
    public AzureSqlIndexerCreateMenuItem(IConfiguration configuration, IPromptHelper promptHelper, ISearchIndexerService indexerService,
        IHotelIndexerService hotelIndexerService)
    {
        _promptHelper = promptHelper;
        _indexerService = indexerService;
        _hotelIndexerService = hotelIndexerService;
 
        _defaultIndexName = configuration["SearchServiceIndexName"];
        _defaultIndexerName = configuration["SearchServiceAzureSqlIndexerName"];
        _defaultDataSourceName = configuration["SearchServiceAzureSqlDataSourceName"];
    }

    public async Task<ConsoleMenuItemResponse> WorkAsync()
    {
        string dataSourceName = _promptHelper.GetText($"Name of the HOTEL Data Source (Default: {_defaultDataSourceName})?", true, true);
        if (string.IsNullOrWhiteSpace(dataSourceName))
            dataSourceName = _defaultDataSourceName;

        if (dataSourceName == "exit")
            return new ConsoleMenuItemResponse(false, false);
        
        string indexName = _promptHelper.GetText($"Name of the Index (Default: {_defaultIndexName})?", true, true);
        if (string.IsNullOrWhiteSpace(indexName))
            indexName = _defaultIndexName;

        if (indexName == "exit")
            return new ConsoleMenuItemResponse(false, false);

        string indexerName = _promptHelper.GetText($"Name of the HOTEL Indexer (Default: {_defaultIndexerName})?", true, true);
        if (string.IsNullOrWhiteSpace(indexerName))
            indexerName = _defaultIndexerName;

        if (indexerName == "exit")
            return new ConsoleMenuItemResponse(false, false);

        Console.WriteLine("Creating the indexer that will use the data source to pull info from the Hotel table...");
        await _hotelIndexerService.CreateIndexerAsync(indexerName, dataSourceName, indexName);
      
        if (_promptHelper.GetYorN("Do you want to run the indexer now?", true))
        {
            Console.WriteLine("Running the index now....");
            await _indexerService.RunIndexerAsync(indexerName);
        }

        Console.WriteLine("-------------------------------");

        return new ConsoleMenuItemResponse(false, false);
    }

    public string ItemText => "Azure SQL Create Hotels Indexer";

    /// <summary>Optional data from the attribute.</summary>
    public string AttributeData { get; set; } = string.Empty;
  

}