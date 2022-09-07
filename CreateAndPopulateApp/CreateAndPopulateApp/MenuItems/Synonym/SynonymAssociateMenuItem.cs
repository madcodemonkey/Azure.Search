using ConsoleMenuHelper;
using Microsoft.Extensions.Configuration;
using Search.Services;

namespace CreateAndPopulateApp;

[ConsoleMenuItem("Synonyms")]
public class SynonymAssociateMenuItem : IConsoleMenuItem
{
    private readonly IPromptHelper _promptHelper;
    private readonly IHotelSynonymService _hotelSynonymService;
    private readonly string _defaultMapName;
    private readonly string _defaultIndexName;

    /// <summary>Constructor</summary>
    public SynonymAssociateMenuItem(IConfiguration configuration, IPromptHelper promptHelper, IHotelSynonymService hotelSynonymService)
    {
        _promptHelper = promptHelper;
        _hotelSynonymService = hotelSynonymService;
        _defaultMapName = configuration["SearchServiceSynonymMapName"];
        _defaultIndexName = configuration["SearchServiceIndexName"];
    }

    public async Task<ConsoleMenuItemResponse> WorkAsync()
    {
        string hotelIndexName = _promptHelper.GetText($"Name of the HOTEL index to associate to a synonym map (Default: {_defaultIndexName})?", true, true);
        if (string.IsNullOrWhiteSpace(hotelIndexName))
            hotelIndexName = _defaultIndexName;


        string synonymMapName = _promptHelper.GetText($"Name of the synonym map to associate to the HOTEL index (Default: {_defaultMapName})?", true, true);
        if (string.IsNullOrWhiteSpace(synonymMapName))
            synonymMapName = _defaultMapName;
       
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