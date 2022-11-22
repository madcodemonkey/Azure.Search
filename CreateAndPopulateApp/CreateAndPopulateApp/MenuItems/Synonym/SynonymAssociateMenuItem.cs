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
        Console.WriteLine($"HOTEL index name to associate to a synonym map: {_settings.Hotel.IndexName}");
        Console.WriteLine($"Synonym map to associate to the HOTEL index: {_settings.Synonyms.HotelMapName}");
        Console.WriteLine("  ");
        if (_promptHelper.GetYorN("Do you want to associate this synonym map with this index?", true))
        {
            await _hotelSynonymService.AssociateSynonymMapToHotelFieldsAsync();
        }

        Console.WriteLine("-------------------------------");

        return new ConsoleMenuItemResponse(false, false);
    }

    public string ItemText => "Associate index to synonym map";

    /// <summary>Optional data from the attribute.</summary>
    public string AttributeData { get; set; } = string.Empty;
}