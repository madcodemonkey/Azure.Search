using ConsoleMenuHelper;
using Search.Services;

namespace CreateAndPopulateApp;

[ConsoleMenuItem("Index", 6)]
public class Query6HotelIndexMenuItem : QueryHotelIndexMenuItemBase, IConsoleMenuItem
{
    private readonly ISearchIndexService _indexService;
    private readonly SearchServiceSettings _settings;
    private readonly IPromptHelper _promptHelper;

    public Query6HotelIndexMenuItem(ISearchIndexService indexService, SearchServiceSettings settings, IPromptHelper promptHelper)
    {
        _indexService = indexService;
        _settings = settings;
        _promptHelper = promptHelper;
    }

    public async Task<ConsoleMenuItemResponse> WorkAsync()
    {
        string indexName = _promptHelper.GetText($"Name of the index to search (Default: {_settings.HotelIndexName})?", true, true);
        if (string.IsNullOrWhiteSpace(indexName))
            indexName = _settings.HotelIndexName;

        while (true)
        {
            string searchText = _promptHelper.GetText("The search text (Default: 'sec' or type 'exit')?", true, true);
            if (searchText.ToLower() == "exit")
                break;

            if (string.IsNullOrWhiteSpace(searchText))
                searchText = "sec";

            Console.WriteLine($"Query #6: Call Autocomplete on HotelName that starts with '{searchText}'...");

            var autoresponse = await _indexService.AutocompleteAsync(indexName, searchText, "sg");
            WriteDocumentsAsync(autoresponse);
            Console.WriteLine("-------------------------------");
        }



        return new ConsoleMenuItemResponse(false, false);
    }

    public string ItemText => "Query #6: Call Autocomplete on HotelName that starts with ";

    /// <summary>Optional data from the attribute.</summary>
    public string AttributeData { get; set; } = string.Empty;
}