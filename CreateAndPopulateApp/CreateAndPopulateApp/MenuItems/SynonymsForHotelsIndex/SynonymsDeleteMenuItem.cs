using ConsoleMenuHelper;
using Microsoft.Extensions.Configuration;
using Search.Services;

namespace CreateAndPopulateApp;

[ConsoleMenuItem("Synonyms")]
public class SynonymsDeleteMenuItem : IConsoleMenuItem
{
    private readonly IPromptHelper _promptHelper;
    private readonly ISearchSynonymService _synonymService;
    private readonly string _defaultMapName;

    public SynonymsDeleteMenuItem(IConfiguration configuration, IPromptHelper promptHelper, ISearchSynonymService synonymService)
    {
        _promptHelper = promptHelper;
        _synonymService = synonymService;
        _defaultMapName = configuration["SearchServiceSynonymMapName"];
    }

    public async Task<ConsoleMenuItemResponse> WorkAsync()
    {
        var synonymMapName = _promptHelper.GetText($"Name of the synonym map to delete (Default: '{_defaultMapName}')?", false);

        if (synonymMapName != "exit")
        {
            await _synonymService.DeleteAsync(synonymMapName);
        }

        Console.WriteLine("-------------------------------");

        return new ConsoleMenuItemResponse(false, false);

    }

    public string ItemText => "Delete Synonyms";

    /// <summary>Optional data from the attribute.</summary>
    public string AttributeData { get; set; }
}