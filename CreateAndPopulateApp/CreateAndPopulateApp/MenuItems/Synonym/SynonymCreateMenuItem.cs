using ConsoleMenuHelper;
using Search.CogServices;
using Search.Services;

namespace CreateAndPopulateApp;

[ConsoleMenuItem("Synonyms")]
public class SynonymCreateMenuItem : IConsoleMenuItem
{
    private readonly SearchServiceSettings _settings;
    private readonly IPromptHelper _promptHelper;
    private readonly IAcmeSearchSynonymService _synonymService;

    /// <summary>Constructor</summary>
    public SynonymCreateMenuItem(SearchServiceSettings settings, IPromptHelper promptHelper, IAcmeSearchSynonymService synonymService)
    {
        _settings = settings;
        _promptHelper = promptHelper;
        _synonymService = synonymService;
    }

    public async Task<ConsoleMenuItemResponse> WorkAsync()
    {

        string synonymMapName = _promptHelper.GetText($"Create of the synonym map (Default: {_settings.Synonyms.HotelMapName})?", true, true);
        if (string.IsNullOrWhiteSpace(synonymMapName))
            synonymMapName = _settings.Synonyms.HotelMapName;

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