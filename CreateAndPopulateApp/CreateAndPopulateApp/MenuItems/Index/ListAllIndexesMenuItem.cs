using ConsoleMenuHelper;
using Search.CogServices;

namespace CreateAndPopulateApp;

[ConsoleMenuItem("Index", 8)]
public class ListAllIndexesMenuItem : IConsoleMenuItem
{
    private readonly IAcmeSearchIndexService _indexService;

    public ListAllIndexesMenuItem(IAcmeSearchIndexService indexService)
    {
        _indexService = indexService;
    }

    public async Task<ConsoleMenuItemResponse> WorkAsync()
    {
        foreach (string indexName in await _indexService.GetIndexNamesAsync())
        {
            Console.WriteLine(indexName);
        }


        Console.WriteLine("-------------------------------");

        return new ConsoleMenuItemResponse(false, false);
    }

    public string ItemText => "List all indexes";

    /// <summary>Optional data from the attribute.</summary>
    public string AttributeData { get; set; } = string.Empty;
}