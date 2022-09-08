using ConsoleMenuHelper;
using Microsoft.Extensions.Configuration;
using Search.Services;

namespace CreateAndPopulateApp;

[ConsoleMenuItem("AzureSql", 5)]
public class AzureSqlIndexerDeleteMenuItem : IConsoleMenuItem
{
    private readonly IPromptHelper _promptHelper;
    private readonly ISearchIndexerService _indexerService;
    private readonly string _defaultIndexerName;

    /// <summary>Constructor</summary>
    public AzureSqlIndexerDeleteMenuItem(IConfiguration configuration, IPromptHelper promptHelper, ISearchIndexerService indexerService)
    {
        _promptHelper = promptHelper;
        _indexerService = indexerService;
     
        _defaultIndexerName = configuration["SearchServiceAzureSqlIndexerName"];
    }

    public async Task<ConsoleMenuItemResponse> WorkAsync()
    {
        string indexerName = _promptHelper.GetText($"Name of the Indexer to delete (Default: {_defaultIndexerName})?", true, true);
        if (string.IsNullOrWhiteSpace(indexerName))
            indexerName = _defaultIndexerName;

        if (indexerName == "exit")
            return new ConsoleMenuItemResponse(false, false);
        
        var result = await _indexerService.DeleteIndexerAsync(indexerName);
        if (result)
            Console.WriteLine($"Deleted {indexerName}");
        else  Console.WriteLine("NOTHING deleted");

        Console.WriteLine("-------------------------------");

        return new ConsoleMenuItemResponse(false, false);
    }

    public string ItemText => "Indexer delete";

    /// <summary>Optional data from the attribute.</summary>
    public string AttributeData { get; set; } = string.Empty;
}