﻿using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using Search.CogServices;
using Search.Model;

namespace Search.Services;

public class HotelSuggestorService : AcmeSuggestorServiceBase<HotelSuggestorResult, HotelDocument>, IHotelSuggestorService
{
    private readonly SearchServiceSettings _settings;

    /// <summary>Constructor</summary>
    public HotelSuggestorService(SearchServiceSettings settings,
        IAcmeSearchIndexService searchIndexService,
        IHotelFilterService filterService) : base(searchIndexService, filterService)
    {
        _settings = settings;
    }

    protected override string IndexName => _settings.Hotel.IndexName;
    protected override string SuggestorName => _settings.Hotel.SuggestorName;


    /// <summary>Converts the results of calling the Azure Search API SuggestAsync method to a custom result.</summary>
    /// <param name="azSuggestResults">The Azure Search API methods return result</param>
    protected override List<HotelSuggestorResult> ConvertResults(SuggestResults<HotelDocument> azSuggestResults)
    {
        var result = new List<HotelSuggestorResult>();

        foreach (SearchSuggestion<HotelDocument>? azSuggestion in azSuggestResults.Results)
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
            nameof(HotelDocument.HotelName).ConvertToCamelCase(),
            nameof(HotelDocument.Category).ConvertToCamelCase()
        };
    }

    /// <summary>Creates a set of default options you can then override if necessary.</summary>
    /// <param name="request">The request from the user.</param>
    /// <param name="rolesTheUserIsAssigned">The roles assigned to the user</param>
    protected override SuggestOptions CreateDefaultOptions(AcmeSearchQuery request, List<string> rolesTheUserIsAssigned)
    {
        var result = base.CreateDefaultOptions(request, rolesTheUserIsAssigned);
        
        result.Select.Clear();
        result.Select.Add(nameof(HotelDocument.HotelName).ConvertToCamelCase());

        return result;
    }
}