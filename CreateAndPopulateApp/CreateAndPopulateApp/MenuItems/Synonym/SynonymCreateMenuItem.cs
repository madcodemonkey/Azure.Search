using ConsoleMenuHelper;
using Microsoft.Extensions.Configuration;
using Search.Services;

namespace CreateAndPopulateApp;

[ConsoleMenuItem("Synonyms")]
public class SynonymCreateMenuItem : IConsoleMenuItem
{
    private readonly IPromptHelper _promptHelper;
    private readonly ISearchSynonymService _synonymService;
    private readonly string _defaultMapName;

    /// <summary>Constructor</summary>
    public SynonymCreateMenuItem(IConfiguration configuration, IPromptHelper promptHelper, ISearchSynonymService synonymService)
    {
        _promptHelper = promptHelper;
        _synonymService = synonymService;
        _defaultMapName = configuration["SearchServiceSynonymMapName"];
    }

    public async Task<ConsoleMenuItemResponse> WorkAsync()
    {
        string synonymMapName = _promptHelper.GetText($"Create of the synonym map (Default: {_defaultMapName})?", true, true);
        if (string.IsNullOrWhiteSpace(synonymMapName))
            synonymMapName = _defaultMapName;
       
        if (synonymMapName != "exit")
        {
            // Note that each synonym group is new line delimited!
            // Docs to understand equivalency: USA, United States, United States of America
            // https://learn.microsoft.com/en-us/azure/search/search-synonyms#equivalency-rules
            // Docs to understand explicit mapping (substitute all the words on the left with one on right):  Washington, Wash., WA => WA
            // https://learn.microsoft.com/en-us/azure/search/search-synonyms#explicit-mapping
            await _synonymService.CreateAsync(synonymMapName, "hotel, motel\ninternet,wifi\nfive star=>luxury\neconomy,inexpensive=>budget");
        }

        Console.WriteLine("-------------------------------");

        return new ConsoleMenuItemResponse(false, false);
    }

    public string ItemText => "Create Synonyms";

    /// <summary>Optional data from the attribute.</summary>
    public string AttributeData { get; set; } = string.Empty;
}