using ConsoleMenuHelper;
using Search.Services;

namespace CreateAndPopulateApp;

[ConsoleMenuItem("Synonyms")]
public class SynonymListAllMapsMenuItem : IConsoleMenuItem
{
    private readonly ISearchSynonymService _synonymService;

    /// <summary>Constructor</summary>
    public SynonymListAllMapsMenuItem(ISearchSynonymService synonymService)
    {
        _synonymService = synonymService;
    }

    public async Task<ConsoleMenuItemResponse> WorkAsync()
    {
        var response  = await _synonymService.GetSynonymMapNamesAsync();
        foreach (string item in response)
        {
            Console.WriteLine(item);
        }
        
        Console.WriteLine("-------------------------------");

        return new ConsoleMenuItemResponse(false, false);
    }

    public string ItemText => "List all Synonyms Maps";

    /// <summary>Optional data from the attribute.</summary>
    public string AttributeData { get; set; } = string.Empty;
}