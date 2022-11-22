using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using ConsoleMenuHelper;
using Search.CogServices;
using Search.Model;
using Search.Services;
using StringExtension = Search.Services.StringExtension;

namespace CreateAndPopulateApp;

[ConsoleMenuItem("Index", 3)]
public class Query3HotelIndexMenuItem : QueryHotelIndexMenuItemBase, IConsoleMenuItem
{
    private readonly IAcmeSearchIndexService _indexService;
    private readonly SearchServiceSettings _settings;
    private readonly IPromptHelper _promptHelper;

    public Query3HotelIndexMenuItem(IAcmeSearchIndexService indexService, SearchServiceSettings settings, IPromptHelper promptHelper)
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

        string searchText = _promptHelper.GetText("Search text (Default: 'pool')?", true, true);
        if (string.IsNullOrWhiteSpace(searchText))
            searchText = "pool";

        Console.WriteLine($"Query #3: Limit search to the tags field ('{searchText}' in the tags field)");

        // Be careful here. The case must match the field names we used in the index.
        // Since I've lowercased my first characters with the [JsonPropertyName("hotelId")] attribute, it expects them to match.
        var options = new SearchOptions
        {
            SearchFields = { StringExtension.ConvertToCamelCase(nameof(HotelDocument.Tags)) }
        };

        options.Select.Add(StringExtension.ConvertToCamelCase(nameof(HotelDocument.HotelId)));
        options.Select.Add(StringExtension.ConvertToCamelCase(nameof(HotelDocument.HotelName)));
        options.Select.Add(StringExtension.ConvertToCamelCase(nameof(HotelDocument.Tags)));

        SearchResults<HotelDocument> response = await _indexService.Search<HotelDocument>(indexName, searchText, options);

        await WriteDocumentsAsync(response);

        Console.WriteLine("-------------------------------");

        return new ConsoleMenuItemResponse(false, false);
    }

    public string ItemText => "Query #3: Limit search to the tags field";

    /// <summary>Optional data from the attribute.</summary>
    public string AttributeData { get; set; } = string.Empty;
}