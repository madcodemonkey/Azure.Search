using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using ConsoleMenuHelper;
using Microsoft.Extensions.Configuration;
using Search.Model;
using Search.Services;

namespace CreateAndPopulateApp;

[ConsoleMenuItem("Index", 1)]
public class Query1HotelIndexMenuItem : QueryHotelIndexMenuItemBase, IConsoleMenuItem
{
    private readonly ISearchIndexService _indexService;
    private readonly IPromptHelper _promptHelper;
    private readonly string _defaultIndexName;

    public Query1HotelIndexMenuItem(ISearchIndexService indexService, IConfiguration configuration, IPromptHelper promptHelper)
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

        Console.WriteLine("Query #1: Search on empty term '*' to return all documents, showing a subset of fields...\n");

        var options = new SearchOptions
        {
            IncludeTotalCount = true,
            Filter = "",
            OrderBy = { "" }
        };

        // Be careful here. The case must match what is in the index.  Since I've lowercased my first characters with the 
        // [JsonPropertyName("hotelId")] attribute, it expects them to match.
        options.Select.Add("hotelId");
        options.Select.Add("hotelName");
        options.Select.Add("rating");

        SearchResults<Hotel> response = await _indexService.Search<Hotel>(indexName, "*", options);

        await WriteDocumentsAsync(response);

        Console.WriteLine("-------------------------------");

        return new ConsoleMenuItemResponse(false, false);
    }

    public string ItemText => "Query #1: Search on empty term '*' to return all documents";

    /// <summary>Optional data from the attribute.</summary>
    public string AttributeData { get; set; } = string.Empty;
}