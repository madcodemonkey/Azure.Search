﻿using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using Search.CogServices.Extensions;

namespace Search.CogServices;

public class AcmeSuggestService : IAcmeSuggestService
{
    private readonly IAcmeODataService _oDataService;
    private readonly IAcmeSearchIndexService _searchIndexService;

    /// <summary>Constructor</summary>
    public AcmeSuggestService(IAcmeSearchIndexService searchIndexService,
        IAcmeODataService oDataService)
    {
        _oDataService = oDataService;
        _searchIndexService = searchIndexService;
    }

    /// <summary>Creates a set of default options you can then override if necessary.</summary>
    /// <param name="request">The request from the user.</param>
    /// <param name="securityTrimmingFilter">An optional security trimming filter.</param>
    public virtual SuggestOptions CreateDefaultOptions(AcmeSuggestQuery request, 
        IAcmeSecurityTrimmingFilter? securityTrimmingFilter = null)
    {
        string? filter = _oDataService.BuildODataFilter(request.IndexName, request.Filters, securityTrimmingFilter);

        var options = new SuggestOptions
        {
            Filter = filter,
            HighlightPreTag = request.HighlightPreTag,
            HighlightPostTag = request.HighlightPostTag,
            // MinimumCoverage = 33.3,
            Size = request.NumberOfSuggestionsToRetrieve,
            UseFuzzyMatching = request.UseFuzzyMatching // false by default for performance reasons
        };

        if (request.DocumentFields != null)
        {
            foreach (string fieldName in request.DocumentFields)
            {
                options.Select.Add(fieldName);
            }
        }

        if (request.OrderByFields != null && request.OrderByFields.Count > 0)
        {
            foreach (AcmeSearchOrderBy orderBy in request.OrderByFields)
            {
                string sortOrder = orderBy.SortDescending ? "desc" : "asc";
                options.OrderBy.Add($"{orderBy.FieldName} {sortOrder}");
            }
        }
        else
        {
            options.OrderBy.Add("search.score() desc");
        }

        if (request.SearchFields != null)
        {
            foreach (string fieldName in request.SearchFields)
            {
                options.SearchFields.Add(fieldName);
            }
        }

        return options;
    }

    /// <summary>Suggest</summary>
    /// <param name="request">A request for a suggestion</param>
    /// <param name="securityTrimmingFilter">An optional security trimming filter.</param>
    /// <returns>List of suggestions</returns>
    public virtual async Task<SuggestResults<SearchDocument>> SuggestAsync(AcmeSuggestQuery request,
        IAcmeSecurityTrimmingFilter? securityTrimmingFilter = null)
    {
        var options = CreateDefaultOptions(request, securityTrimmingFilter);

        return await SuggestAsync(request, options, securityTrimmingFilter?.FieldName);
    }

    /// <summary>Suggest</summary>
    /// <param name="request">A request for a suggestion</param>
    /// <param name="options">The search options to apply</param>
    /// <param name="securityTrimmingFieldName">The name of the field (as specified in the Azure Index and it is case sensitive)
    /// being used for security trimming.  It's needed here to remove it from the document results.</param>
    /// <returns>List of suggestions</returns>
    public virtual async Task<SuggestResults<SearchDocument>> SuggestAsync(AcmeSuggestQuery request, SuggestOptions options,
        string? securityTrimmingFieldName)
    {
        var suggestResult = await _searchIndexService.SuggestAsync<SearchDocument>(request.IndexName, request.Query, request.SuggestorName, options);

        if (securityTrimmingFieldName != null)
        {
            foreach (SearchSuggestion<SearchDocument> item in suggestResult.Results)
            {
                item.Document.RemoveField(securityTrimmingFieldName);
                item.Document.ReMapFields(request.DocumentFieldMaps);
            }
        }

        return suggestResult;
    }
}