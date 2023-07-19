using ConsoleMenuHelper;
using Search.CogServices;
using Search.Services;

namespace CreateAndPopulateApp;

[ConsoleMenuItem("Index", 11)]
public class DeleteHotelsIndexDocumentsMenuItem : IConsoleMenuItem
{
    private readonly IAcmeSearchIndexService _indexService;
    private readonly SearchServiceSettings _settings;
    private readonly IPromptHelper _promptHelper;

    public DeleteHotelsIndexDocumentsMenuItem(IAcmeSearchIndexService indexService, SearchServiceSettings settings, IPromptHelper promptHelper)
    {
        _indexService = indexService;
        _settings = settings;
        _promptHelper = promptHelper;
    }

    public async Task<ConsoleMenuItemResponse> WorkAsync()
    {
        string indexName = _promptHelper.GetText($"Name of the index to clear (Default: {_settings.Hotel.IndexName})?", true, true);
        if (string.IsNullOrWhiteSpace(indexName))
            indexName = _settings.Hotel.IndexName;

        if (indexName != "exit")
        {
            if (_promptHelper.GetYorN($"WARNING!  Are you sure you want to delete all the documents inside of the '{indexName}' index?", true))
            {
                long numberDeleted = await _indexService.ClearAllDocumentsAsync(indexName);
                Console.WriteLine($"Deleted {numberDeleted} documents");
            }
        }

        Console.WriteLine("-------------------------------");

        return new ConsoleMenuItemResponse(false, false);
    }

    public string ItemText => "Clear/Delete all documents in the Index";

    /// <summary>Optional data from the attribute.</summary>
    public string AttributeData { get; set; } = string.Empty;
}