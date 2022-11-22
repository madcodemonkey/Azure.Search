using ConsoleMenuHelper;
using Search.CogServices;

namespace CreateAndPopulateApp;

[ConsoleMenuItem("AzureSql", 4)]
public class AzureSqlIndexerListMenuItem : IConsoleMenuItem
{
    private readonly IAcmeSearchIndexerService _indexerService;

    /// <summary>Constructor</summary>
    public AzureSqlIndexerListMenuItem(IAcmeSearchIndexerService indexerService)
    {
        _indexerService = indexerService;
    }

    public async Task<ConsoleMenuItemResponse> WorkAsync()
    {
        List<string> indexerList = await _indexerService.GetListAsync();

        if (indexerList.Count == 0)
            Console.WriteLine("Nothing found.");

        foreach (var indexerName in indexerList)
        {
            Console.WriteLine(indexerName);
        }

        Console.WriteLine("-------------------------------");

        return new ConsoleMenuItemResponse(false, false);
    }

    public string ItemText => "Indexer List (show all of them)";

    /// <summary>Optional data from the attribute.</summary>
    public string AttributeData { get; set; } = string.Empty;
}