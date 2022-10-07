using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using ConsoleMenuHelper;
using Microsoft.Extensions.Configuration;
using Search.Model;
using Search.Services;

namespace CreateAndPopulateApp;

[ConsoleMenuItem("Index", 3)]
public class Query3HotelIndexMenuItem : QueryHotelIndexMenuItemBase, IConsoleMenuItem
{
    private readonly ISearchIndexService _indexService;
    private readonly IPromptHelper _promptHelper;
    private readonly string _defaultIndexName;

    public Query3HotelIndexMenuItem(ISearchIndexService indexService, IConfiguration configuration, IPromptHelper promptHelper)
    {
        _indexService = indexService;
        _promptHelper = promptHelper;
        _defaultIndexName = configuration["SearchServiceIndexName"];
    }

    public async Task<ConsoleMenuItemResponse> WorkAsync()
    {
        string indexName = _promptHelper.GetText($"Name of the index to search (Default: {_defaultIndexName})?", true, true);
        if (string.IsNullOrWhiteSpace(indexName))
            indexName = _defaultIndexName;

        string searchText = _promptHelper.GetText("Search text (Default: 'pool')?", true, true);
        if (string.IsNullOrWhiteSpace(searchText))
            searchText = "pool";

        Console.WriteLine($"Query #3: Limit search to the tags field ('{searchText}' in the tags field)");

        // Be careful here. The case must match the field names we used in the index.
        // Since I've lowercased my first characters with the [JsonPropertyName("hotelId")] attribute, it expects them to match.
        var options = new SearchOptions
        {
            SearchFields = { "tags" }
        };

        options.Select.Add("hotelId");
        options.Select.Add("hotelName");
        options.Select.Add("tags");

 
        SearchResults<Hotel> response = await _indexService.Search<Hotel>(indexName, searchText, options);

        await WriteDocumentsAsync(response);

        Console.WriteLine("-------------------------------");

        return new ConsoleMenuItemResponse(false, false);
    }

    public string ItemText => "Query #3: Limit search to the tags field";

    /// <summary>Optional data from the attribute.</summary>
    public string AttributeData { get; set; } = string.Empty;
}