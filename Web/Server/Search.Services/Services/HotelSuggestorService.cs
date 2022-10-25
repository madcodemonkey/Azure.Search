﻿using Azure.Search.Documents.Models;
using Search.Model;

namespace Search.Services;

public class HotelSuggestorService : AcmeSuggestorServiceBase<HotelSuggestorResult, SearchHotel>, IHotelSuggestorService
{
    private readonly SearchServiceSettings _settings;

    /// <summary>Constructor</summary>
    public HotelSuggestorService(SearchServiceSettings settings,
        IAcmeSearchIndexService searchIndexService,
        IHotelFilterService filterService) : base(searchIndexService, filterService)
    {
        _settings = settings;
    }

    protected override string IndexName => _settings.HotelIndexName;
    protected override string SuggestorName => _settings.HotelSuggestorName;


    /// <summary>Converts the results of calling the Azure Search API SuggestAsync method to a custom result.</summary>
    /// <param name="azSuggestResults">The Azure Search API methods return result</param>
    protected override List<HotelSuggestorResult> ConvertResults(SuggestResults<SearchHotel> azSuggestResults)
    {
        var result = new List<HotelSuggestorResult>();

        foreach (SearchSuggestion<SearchHotel>? azSuggestion in azSuggestResults.Results)
        {
            result.Add(new HotelSuggestorResult
            {
                Text = azSuggestion.Text,
                Category = azSuggestion.Document.Category,
                HotelName = azSuggestion.Document.HotelName
            });
        }

        return result;
    }

    /// <summary>Gets a list of fields that we should be returned with the document found with the suggestion. If left blank, it will return the key field.
    /// Warning! Field names must match exactly how they appear in the Azure Search Document!</summary>
    protected override List<string> GetFieldNamesToSelect()
    {
        return new List<string>
        {
            nameof(SearchHotel.HotelName).ConvertToCamelCase(),
            nameof(SearchHotel.Category).ConvertToCamelCase()
        };
    }
}