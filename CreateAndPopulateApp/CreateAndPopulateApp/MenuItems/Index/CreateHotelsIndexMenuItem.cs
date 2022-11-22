using ConsoleMenuHelper;
using Search.Services;

namespace CreateAndPopulateApp;

[ConsoleMenuItem("Index", 9)]
public class CreateHotelsIndexMenuItem : IConsoleMenuItem
{
    private readonly SearchServiceSettings _settings;
    private readonly IHotelIndexService _hotelIndexService;
    private readonly IPromptHelper _promptHelper;

    public CreateHotelsIndexMenuItem(SearchServiceSettings settings, IHotelIndexService hotelIndexService, IPromptHelper promptHelper)
    {
        _settings = settings;
        _hotelIndexService = hotelIndexService;
        _promptHelper = promptHelper;
    }

    public async Task<ConsoleMenuItemResponse> WorkAsync()
    {
        Console.WriteLine($"Name of the index to create or update:: {_settings.Hotel.IndexName}");
        Console.WriteLine("  ");


        if (_promptHelper.GetYorN("Create or update the index?", true))
        {
            await _hotelIndexService.CreateOrUpdateAsync();
        }

        Console.WriteLine("-------------------------------");

        return new ConsoleMenuItemResponse(false, false);
    }

    public string ItemText => "Create Index";

    /// <summary>Optional data from the attribute.</summary>
    public string AttributeData { get; set; } = string.Empty;
}