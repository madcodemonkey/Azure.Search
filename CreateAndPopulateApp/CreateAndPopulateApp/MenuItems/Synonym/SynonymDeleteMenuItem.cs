using ConsoleMenuHelper;
using Search.CogServices;
using Search.Services;

namespace CreateAndPopulateApp;

[ConsoleMenuItem("Synonyms")]
public class SynonymDeleteMenuItem : IConsoleMenuItem
{
    private readonly SearchServiceSettings _settings;
    private readonly IPromptHelper _promptHelper;
    private readonly IAcmeSearchSynonymService _synonymService;

    /// <summary>Constructor</summary>
    public SynonymDeleteMenuItem(SearchServiceSettings settings, IPromptHelper promptHelper, IAcmeSearchSynonymService synonymService)
    {
        _settings = settings;
        _promptHelper = promptHelper;
        _synonymService = synonymService;
    }

    public async Task<ConsoleMenuItemResponse> WorkAsync()
    {
        var synonymMapName = _promptHelper.GetText($"Name of the synonym map to delete (Default: '{_settings.Synonyms.HotelMapName}')?", false);

        if (synonymMapName != "exit")
        {
            await _synonymService.DeleteAsync(synonymMapName);
        }

        Console.WriteLine("-------------------------------");

        return new ConsoleMenuItemResponse(false, false);

    }

    public string ItemText => "Delete Synonyms";

    /// <summary>Optional data from the attribute.</summary>
    public string AttributeData { get; set; } = string.Empty;
}