﻿using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using ConsoleMenuHelper;
using Search.CogServices;
using Search.Model;
using Search.Services;
using StringExtension = Search.Services.StringExtension;

namespace CreateAndPopulateApp;

[ConsoleMenuItem("Index", 2)]
public class Query2HotelIndexMenuItem : QueryHotelIndexMenuItemBase, IConsoleMenuItem
{
    private readonly IAcmeSearchIndexService _indexService;
    private readonly SearchServiceSettings _settings;
    private readonly IPromptHelper _promptHelper;

    public Query2HotelIndexMenuItem(IAcmeSearchIndexService indexService, SearchServiceSettings settings, IPromptHelper promptHelper)
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

        Console.WriteLine("Query #2: Search on 'hotels', filter on 'Rating gt 4', sort by Rating in descending order...\n");

        var ratingField = StringExtension.ConvertToCamelCase(nameof(HotelDocument.Rating));

        var options = new SearchOptions
        {
            Filter = $"{ratingField} gt 4",
            OrderBy = { $"{ratingField} desc" }
        };

        // Be careful here. The case must match what is in the index.  Since I've lowercased my first characters with the 
        // [JsonPropertyName("hotelId")] attribute, it expects them to match.
        options.Select.Add(StringExtension.ConvertToCamelCase(nameof(HotelDocument.HotelId)));
        options.Select.Add(StringExtension.ConvertToCamelCase(nameof(HotelDocument.HotelName)));
        options.Select.Add(StringExtension.ConvertToCamelCase(nameof(HotelDocument.Rating)));


        SearchResults<HotelDocument> response = await _indexService.Search<HotelDocument>(indexName, "*", options);

        await WriteDocumentsAsync(response);

        Console.WriteLine("-------------------------------");

        return new ConsoleMenuItemResponse(false, false);
    }

    public string ItemText => "Query #2: Search on 'hotels', filter on 'Rating gt 4'";

    /// <summary>Optional data from the attribute.</summary>
    public string AttributeData { get; set; } = string.Empty;
}