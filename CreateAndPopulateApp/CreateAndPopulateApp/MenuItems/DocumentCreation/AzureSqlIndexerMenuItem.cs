using ConsoleMenuHelper;
using Microsoft.Extensions.Configuration;
using Search.Services;

namespace CreateAndPopulateApp;

[ConsoleMenuItem("DocumentCreation", 2)]
public class AzureSqlIndexerMenuItem : IConsoleMenuItem
{
    private readonly IPromptHelper _promptHelper;
    private readonly ISearchIndexerService _indexerService;
    private readonly IHotelIndexerService _hotelIndexerService;
    private readonly string _defaultIndexName;
    private readonly string _defaultIndexerName;
    private readonly string _defaultDataSourceName;
    private readonly string _defaultTableName;

    /// <summary>Constructor</summary>
    public AzureSqlIndexerMenuItem(IConfiguration configuration, IPromptHelper promptHelper, ISearchIndexerService indexerService,
        IHotelIndexerService hotelIndexerService)
    {
        _promptHelper = promptHelper;
        _indexerService = indexerService;
        _hotelIndexerService = hotelIndexerService;
        
        _defaultTableName = configuration["SearchServiceAzureSqlTableName"];
        _defaultIndexName = configuration["SearchServiceIndexName"];
        _defaultIndexerName = configuration["SearchServiceAzureSqlIndexerName"];
        _defaultDataSourceName = configuration["SearchServiceAzureSqlDataSourceName"];
    }

    public async Task<ConsoleMenuItemResponse> WorkAsync()
    {
        string dataSourceName = await CreateDataSourceAsync();

        if (dataSourceName == "exit")
            return new ConsoleMenuItemResponse(false, false);
        
        string indexerName = await CreateIndexerAsync(dataSourceName);

        if (indexerName == "exit")
            return new ConsoleMenuItemResponse(false, false);
        
        if (_promptHelper.GetYorN("Do you want to run the indexer now?", true))
        {
            Console.WriteLine("Running the index now....");
            await _indexerService.RunIndexerAsync(indexerName);
        }

        Console.WriteLine("-------------------------------");

        return new ConsoleMenuItemResponse(false, false);
    }

    public string ItemText => "Azure SQL Indexer";

    /// <summary>Optional data from the attribute.</summary>
    public string AttributeData { get; set; } = string.Empty;

    private async Task<string> CreateDataSourceAsync()
    {
        string dataSourceName = _promptHelper.GetText($"Name of the HOTEL Data Source (Default: {_defaultDataSourceName})?", true, true);
        if (string.IsNullOrWhiteSpace(dataSourceName))
            dataSourceName = _defaultDataSourceName;

        if (dataSourceName == "exit")
            return "exit";

        string tableName = _promptHelper.GetText($"Name of the Index (Default: {_defaultTableName})?", true, true);
        if (string.IsNullOrWhiteSpace(tableName))
            tableName = _defaultTableName;

        if (tableName == "exit")
            return "exit";

        Console.WriteLine("Creating the data source that is needed for the indexer...");
        await _indexerService.CreateAzureSqlDataSourceAsync(dataSourceName, tableName);

        return dataSourceName;
    }

    private async Task<string> CreateIndexerAsync(string dataSourceName)
    {
        string indexName = _promptHelper.GetText($"Name of the Index (Default: {_defaultIndexName})?", true, true);
        if (string.IsNullOrWhiteSpace(indexName))
            indexName = _defaultIndexName;

        if (indexName == "exit")
            return "exit";

        string indexerName = _promptHelper.GetText($"Name of the HOTEL Indexer (Default: {_defaultIndexerName})?", true, true);
        if (string.IsNullOrWhiteSpace(indexerName))
            indexerName = _defaultIndexerName;

        if (indexerName == "exit")
            return "exit";

        Console.WriteLine("Creating the indexer that will use the data source to pull info from the Hotel table...");
        await _hotelIndexerService.CreateIndexerAsync(indexerName, dataSourceName, indexName);

        return indexerName;
    }

}