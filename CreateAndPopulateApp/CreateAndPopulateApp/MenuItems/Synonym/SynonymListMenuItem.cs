using ConsoleMenuHelper;
using Microsoft.Extensions.Configuration;
using Search.Services;

namespace CreateAndPopulateApp;

[ConsoleMenuItem("Synonyms")]
public class SynonymListMenuItem : IConsoleMenuItem
{
    private readonly ISearchSynonymService _synonymService;
    private readonly IPromptHelper _promptHelper;
    private readonly string _defaultMapName;

    /// <summary>Constructor</summary>
    public SynonymListMenuItem(IConfiguration configuration, IPromptHelper promptHelper, ISearchSynonymService synonymService)
    {
        _promptHelper = promptHelper;
        _synonymService = synonymService;
        _defaultMapName = configuration["SearchServiceSynonymMapName"];

    }

    public async Task<ConsoleMenuItemResponse> WorkAsync()
    {
        string synonymMapName = _promptHelper.GetText($"Name of the synonym map (Default: {_defaultMapName})?", true, true);
        if (string.IsNullOrWhiteSpace(synonymMapName))
            synonymMapName = _defaultMapName;

        List<string> synonymNames  = await _synonymService.GetSynonymNamesAsync(synonymMapName);
        foreach (string item in synonymNames)
        {
            Console.WriteLine(item);
        }
        
        Console.WriteLine("-------------------------------");

        return new ConsoleMenuItemResponse(false, false);
    }

    public string ItemText => "List Synonyms for one map";

    /// <summary>Optional data from the attribute.</summary>
    public string AttributeData { get; set; } = string.Empty;
}