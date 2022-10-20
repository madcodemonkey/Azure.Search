using ConsoleMenuHelper;
using Search.Services;

namespace CreateAndPopulateApp;

[ConsoleMenuItem("Index", 10)]
public class DeleteHotelsIndexMenuItem : IConsoleMenuItem
{
    private readonly IAcmeSearchIndexService _indexService;
    private readonly SearchServiceSettings _settings;
    private readonly IPromptHelper _promptHelper;

    public DeleteHotelsIndexMenuItem(IAcmeSearchIndexService indexService, SearchServiceSettings settings, IPromptHelper promptHelper)
    {
        _indexService = indexService;
        _settings = settings;
        _promptHelper = promptHelper;
    }

    public async Task<ConsoleMenuItemResponse> WorkAsync()
    {
        string indexName = _promptHelper.GetText($"Name of the index to delete (Default: {_settings.HotelIndexName})?", true, true);
        if (string.IsNullOrWhiteSpace(indexName))
            indexName = _settings.HotelIndexName;

        if (indexName != "exit")
        {
            if (_promptHelper.GetYorN($"WARNING!  Are you sure you want to delete the '{indexName}' index?", true))
            {
                await _indexService.DeleteAsync(indexName);
            }
        }

        Console.WriteLine("-------------------------------");

        return new ConsoleMenuItemResponse(false, false);
    }

    public string ItemText => "Delete Index";

    /// <summary>Optional data from the attribute.</summary>
    public string AttributeData { get; set; } = string.Empty;
}