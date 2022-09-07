using ConsoleMenuHelper;
using Search.Services;

namespace CreateAndPopulateApp;

[ConsoleMenuItem("Index", 1)]
public class ListAllIndexesMenuItem : IConsoleMenuItem
{
    private readonly ISearchIndexService _indexService;

    public ListAllIndexesMenuItem(ISearchIndexService indexService)
    {
        _indexService = indexService;
    }

    public async Task<ConsoleMenuItemResponse> WorkAsync()
    {
        foreach (string indexName in  await  _indexService.GetIndexNamesAsync())
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