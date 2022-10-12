using ConsoleMenuHelper;
using Search.Services;

namespace CreateAndPopulateApp;

[ConsoleMenuItem("Synonyms")]
public class SynonymAssociateMenuItem : IConsoleMenuItem
{
    private readonly SearchServiceSettings _settings;
    private readonly IPromptHelper _promptHelper;
    private readonly IHotelSynonymService _hotelSynonymService;

    /// <summary>Constructor</summary>
    public SynonymAssociateMenuItem(SearchServiceSettings settings, IPromptHelper promptHelper, IHotelSynonymService hotelSynonymService)
    {
        _settings = settings;
        _promptHelper = promptHelper;
        _hotelSynonymService = hotelSynonymService;
    }

    public async Task<ConsoleMenuItemResponse> WorkAsync()
    {
        string hotelIndexName = _promptHelper.GetText($"Name of the HOTEL index to associate to a synonym map (Default: {_settings.HotelIndexName})?", true, true);
        if (string.IsNullOrWhiteSpace(hotelIndexName))
            hotelIndexName = _settings.HotelIndexName;
        
        string synonymMapName = _promptHelper.GetText($"Name of the synonym map to associate to the HOTEL index (Default: {_settings.HotelSynonymMapName})?", true, true);
        if (string.IsNullOrWhiteSpace(synonymMapName))
            synonymMapName = _settings.HotelSynonymMapName;

        if (synonymMapName != "exit")
        {
            await _hotelSynonymService.AssociateSynonymMapToHotelFieldsAsync(hotelIndexName, synonymMapName);
        }

        Console.WriteLine("-------------------------------");

        return new ConsoleMenuItemResponse(false, false);
    }

    public string ItemText => "Associate index to synonym map";

    /// <summary>Optional data from the attribute.</summary>
    public string AttributeData { get; set; } = string.Empty;
}