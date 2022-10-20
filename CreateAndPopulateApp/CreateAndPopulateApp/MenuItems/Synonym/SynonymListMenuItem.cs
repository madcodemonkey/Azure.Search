using ConsoleMenuHelper;
using Search.Services;

namespace CreateAndPopulateApp;

[ConsoleMenuItem("Synonyms")]
public class SynonymListMenuItem : IConsoleMenuItem
{
    private readonly IAcmeSearchSynonymService _synonymService;
    private readonly SearchServiceSettings _settings;
    private readonly IPromptHelper _promptHelper;

    /// <summary>Constructor</summary>
    public SynonymListMenuItem(SearchServiceSettings settings, IPromptHelper promptHelper, IAcmeSearchSynonymService synonymService)
    {
        _settings = settings;
        _promptHelper = promptHelper;
        _synonymService = synonymService;
    }

    public async Task<ConsoleMenuItemResponse> WorkAsync()
    {
        string synonymMapName = _promptHelper.GetText($"Name of the synonym map (Default: {_settings.HotelSynonymMapName})?", true, true);
        if (string.IsNullOrWhiteSpace(synonymMapName))
            synonymMapName = _settings.HotelSynonymMapName;

        List<string> synonymNames = await _synonymService.GetSynonymNamesAsync(synonymMapName);
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