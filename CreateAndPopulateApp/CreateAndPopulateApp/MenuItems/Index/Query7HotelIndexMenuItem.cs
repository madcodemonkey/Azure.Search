using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using ConsoleMenuHelper;
using Microsoft.Extensions.Configuration;
using Search.Model;
using Search.Services;

namespace CreateAndPopulateApp;

[ConsoleMenuItem("Index", 7)]
public class Query7HotelIndexMenuItem : QueryHotelIndexMenuItemBase, IConsoleMenuItem
{
    private readonly ISearchIndexService _indexService;
    private readonly IPromptHelper _promptHelper;
    private readonly string _defaultIndexName;

    public Query7HotelIndexMenuItem(ISearchIndexService indexService, IConfiguration configuration, IPromptHelper promptHelper)
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

        string searchText = _promptHelper.GetText("Search text (Default: 'hotel')?", true, true);
        if (string.IsNullOrWhiteSpace(searchText))
            searchText = "hotel";
        
        
        var options = new SearchOptions
        {
            IncludeTotalCount = true,
            Filter = "",
            OrderBy = { "search.score() desc" }
        };

        // Be careful here. The case must match what is in the index.  Since I've lowercased my first characters with the 
        // [JsonPropertyName("hotelId")] attribute, it expects them to match.
        options.Select.Add("hotelId");
        options.Select.Add("hotelName");
        options.Select.Add("rating");

        await DoQueryWithoutScoringProfile(indexName, searchText, options);
        await DoQueryWithScoringProfile(indexName, searchText, options);

        Console.WriteLine("-------------------------------");

        return new ConsoleMenuItemResponse(false, false);
    }

    private async Task DoQueryWithoutScoringProfile(string indexName, string searchText, SearchOptions options)
    {
        Console.WriteLine("Query #7: Without scoring profile...");

        SearchResults<Hotel> response = await _indexService.Search<Hotel>(indexName, searchText, options);

        await WriteDocumentsAsync(response);
    }

    
    private async Task DoQueryWithScoringProfile(string indexName, string searchText, SearchOptions options)
    {
        string scoringProfileName = _promptHelper.GetText("Scoring profile name (Default: 'sp-hotel-name')?", true, true);
        if (string.IsNullOrWhiteSpace(scoringProfileName))
            scoringProfileName = "sp-hotel-name";

        Console.WriteLine("Query #7: With scoring profile...");

        options.ScoringProfile = scoringProfileName;

        SearchResults<Hotel> response = await _indexService.Search<Hotel>(indexName, searchText, options);

        await WriteDocumentsAsync(response);
    }

    public string ItemText => "Query #7: With and without scoring profile.";

    /// <summary>Optional data from the attribute.</summary>
    public string AttributeData { get; set; } = string.Empty;
}