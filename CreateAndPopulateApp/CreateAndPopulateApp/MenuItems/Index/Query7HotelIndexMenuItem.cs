using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using ConsoleMenuHelper;
using Search.CogServices;
using Search.Model;
using Search.Services;
using StringExtension = Search.Services.StringExtension;

namespace CreateAndPopulateApp;

[ConsoleMenuItem("Index", 7)]
public class Query7HotelIndexMenuItem : QueryHotelIndexMenuItemBase, IConsoleMenuItem
{
    private readonly IAcmeSearchIndexService _indexService;
    private readonly SearchServiceSettings _settings;
    private readonly IPromptHelper _promptHelper;

    public Query7HotelIndexMenuItem(IAcmeSearchIndexService indexService, SearchServiceSettings settings, IPromptHelper promptHelper)
    {
        _indexService = indexService;
        _settings = settings;
        _promptHelper = promptHelper;
    }

    public async Task<ConsoleMenuItemResponse> WorkAsync()
    {
        string indexName = _promptHelper.GetText($"Name of the index to search (Default: {_settings.Hotel.IndexName})?", true, true);
        if (string.IsNullOrWhiteSpace(indexName))
            indexName = _settings.Hotel.IndexName;

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
        options.Select.Add(StringExtension.ConvertToCamelCase(nameof(HotelDocument.HotelId)));
        options.Select.Add(StringExtension.ConvertToCamelCase(nameof(HotelDocument.HotelName)));
        options.Select.Add(StringExtension.ConvertToCamelCase(nameof(HotelDocument.Rating)));


        await DoQueryWithoutScoringProfile(indexName, searchText, options);
        await DoQueryWithScoringProfile(indexName, searchText, options);

        Console.WriteLine("-------------------------------");

        return new ConsoleMenuItemResponse(false, false);
    }

    private async Task DoQueryWithoutScoringProfile(string indexName, string searchText, SearchOptions options)
    {
        Console.WriteLine("Query #7: Without scoring profile...");

        SearchResults<HotelDocument> response = await _indexService.Search<HotelDocument>(indexName, searchText, options);

        await WriteDocumentsAsync(response);
    }


    private async Task DoQueryWithScoringProfile(string indexName, string searchText, SearchOptions options)
    {
        string scoringProfileName = _promptHelper.GetText("Scoring profile name (Default: 'sp-hotel-name')?", true, true);
        if (string.IsNullOrWhiteSpace(scoringProfileName))
            scoringProfileName = "sp-hotel-name";

        Console.WriteLine("Query #7: With scoring profile...");

        options.ScoringProfile = scoringProfileName;

        SearchResults<HotelDocument> response = await _indexService.Search<HotelDocument>(indexName, searchText, options);

        await WriteDocumentsAsync(response);
    }

    public string ItemText => "Query #7: With and without scoring profile.";

    /// <summary>Optional data from the attribute.</summary>
    public string AttributeData { get; set; } = string.Empty;
}