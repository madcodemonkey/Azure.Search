using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using ConsoleMenuHelper;
using Microsoft.Extensions.Configuration;
using Search.Model;
using Search.Services;

namespace CreateAndPopulateApp;

[ConsoleMenuItem("Index", 4)]
public class Query4HotelIndexMenuItem : QueryHotelIndexMenuItemBase, IConsoleMenuItem
{
    private readonly ISearchIndexService _indexService;
    private readonly IPromptHelper _promptHelper;
    private readonly string _defaultIndexName;

    public Query4HotelIndexMenuItem(ISearchIndexService indexService, IConfiguration configuration, IPromptHelper promptHelper)
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

        string searchText = _promptHelper.GetText($"Search text (Default: '*')?", true, true);
        if (string.IsNullOrWhiteSpace(searchText))
            searchText = "*";

        Console.WriteLine("Query #4: Facet on 'Category'...\n");

        var options = new SearchOptions
        {
            Filter = ""
        };

        // Be careful here. The case must match the field names we used in the index.
        // Since I've lowercased my first characters with the [JsonPropertyName("hotelId")] attribute, it expects them to match.
        options.Facets.Add("category");
        options.Facets.Add("tags");

        options.Select.Add("hotelId");
        options.Select.Add("hotelName");
        options.Select.Add("category");
        options.Select.Add("tags");


        SearchResults<Hotel> response = await _indexService.Search<Hotel>(indexName, searchText, options);

     
        await WriteDocumentsAsync(response);

        Console.WriteLine("-------------------------------");

        return new ConsoleMenuItemResponse(false, false);
    }

    public string ItemText => "Query #4: Facet on 'Category'";

    /// <summary>Optional data from the attribute.</summary>
    public string AttributeData { get; set; } = string.Empty;
}