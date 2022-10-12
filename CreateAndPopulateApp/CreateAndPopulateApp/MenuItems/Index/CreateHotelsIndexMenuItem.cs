using ConsoleMenuHelper;
using Search.Model;
using Search.Services;

namespace CreateAndPopulateApp;

[ConsoleMenuItem("Index", 9)]
public class CreateHotelsIndexMenuItem : IConsoleMenuItem
{
    private readonly SearchServiceSettings _settings;
    private readonly ISearchIndexService _indexService;
    private readonly IPromptHelper _promptHelper;

    public CreateHotelsIndexMenuItem(SearchServiceSettings settings, ISearchIndexService indexService, IPromptHelper promptHelper)
    {
        _settings = settings;
        _indexService = indexService;
        _promptHelper = promptHelper;
    }

    public async Task<ConsoleMenuItemResponse> WorkAsync()
    {
        
        string indexName = _promptHelper.GetText($"Name of the index to create or update (Default: {_settings.HotelIndexName})?", true, true);
        if (string.IsNullOrWhiteSpace(indexName))
            indexName = _settings.HotelIndexName;

        if (indexName != "exit")
        {
            await _indexService.CreateOrUpdateAsync(typeof(Hotel), indexName);
        }
        
        Console.WriteLine("-------------------------------");

        return new ConsoleMenuItemResponse(false, false);
    }

    public string ItemText => "Create Index";

    /// <summary>Optional data from the attribute.</summary>
    public string AttributeData { get; set; } = string.Empty;
}