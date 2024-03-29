﻿using System.Text;
using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;

namespace Search.CogServices;

public abstract class AcmeSearchServiceBase<TIndexClass> where TIndexClass : class
{
    /// <summary>Constructor</summary>
    protected AcmeSearchServiceBase(IAcmeSearchIndexService searchIndexService, IAcmeFieldService fieldService)
    {
        SearchIndexService = searchIndexService;
        FieldService = fieldService;
    }

    /// <summary>The Search index service, which is a wrapper around Microsoft's SearchIndexClient class.</summary>
    protected IAcmeSearchIndexService SearchIndexService { get; }

    /// <summary>A service to help us handle fields.  It also adds facets and helps build filters.</summary>
    protected IAcmeFieldService FieldService { get; }

    /// <summary>The name of the index we are querying.</summary>
    protected abstract string IndexName { get; }

    /// <summary>Searches using the Azure Search API, but this overload gives you more control over the options passed to Azure Search.</summary>
    /// <param name="request">The request from the user.</param>
    /// <param name="options">The search options to apply.  It is recommended that you can the <see cref="CreateDefaultOptions"/> method to
    /// get the defaults and then override things that you want to override.</param>
    public virtual async Task<AcmeSearchQueryResult<SearchResult<TIndexClass>>> SearchAsync(AcmeSearchQuery request, SearchOptions options)
    {
        var azSearchResult = await SearchIndexService.SearchAsync<TIndexClass>(IndexName, request.Query, options);

        var result = await WrapResultsAsync(request, azSearchResult);

        return result;
    }

    /// <summary>Searches using the Azure Search API.</summary>
    /// <param name="request">The request from the user.</param>
    /// <param name="rolesTheUserIsAssigned">Case sensitive list of roles that for search trimming.</param>
    public virtual async Task<AcmeSearchQueryResult<SearchResult<TIndexClass>>> SearchAsync(AcmeSearchQuery request, List<string> rolesTheUserIsAssigned)
    {
        SearchOptions options = CreateDefaultOptions(request, rolesTheUserIsAssigned);

        var azSearchResult = await SearchIndexService.SearchAsync<TIndexClass>(IndexName, request.Query, options);

        var result = await WrapResultsAsync(request, azSearchResult);

        return result;
    }

    
    /// <summary>Wraps the search results that came back from the Azure Search Index in a <see cref="AcmeSearchQueryResult"/> instance.
    /// You will still get the raw result, but with additional information that you can return the the client.</summary>
    /// <param name="request">The search request from the client side.</param>
    /// <param name="azSearchResult">The response from the Azure Search index.</param>
    protected virtual async Task<AcmeSearchQueryResult<SearchResult<TIndexClass>>> WrapResultsAsync(AcmeSearchQuery request, Response<SearchResults<TIndexClass>> azSearchResult)
    {
        var result = new AcmeSearchQueryResult<SearchResult<TIndexClass>>
        {
            Query = request.Query,
            Filters = request.Filters,
            OrderByFields = request.OrderByFields,
            Facets = FieldService.ConvertFacets(azSearchResult.Value.Facets, request.Filters),
            IncludeAllWords = request.IncludeAllWords,
            IncludeCount = request.IncludeCount,
            TotalCount = azSearchResult.Value.TotalCount ?? 0,
            ItemsPerPage = request.ItemsPerPage,
            PageNumber = request.PageNumber,
            Docs = await GetPagedResultsAsync(azSearchResult.Value),
        };

        return result;
    }

    /// <summary>Gets the results requested and will page the results out of Azure Search. This method is called by <see cref="WrapResultsAsync"/> </summary>
    /// <param name="azSearchResults">The search results from the call to the Azure Search PAI.</param>
    protected virtual async Task<List<SearchResult<TIndexClass>>> GetPagedResultsAsync(SearchResults<TIndexClass> azSearchResults)
    {
        var result = new List<SearchResult<TIndexClass>>();

        AsyncPageable<SearchResult<TIndexClass>> azOnePageOfSearchDocuments = azSearchResults.GetResultsAsync();

        await foreach (SearchResult<TIndexClass> item in azOnePageOfSearchDocuments)
        {
            result.Add(item);
        }

        return result;
    }

   
    /// <summary>Creates a set of default options you can then override if necessary.</summary>
    /// <param name="request">The request from the user.</param>
    /// <param name="rolesTheUserIsAssigned">The roles assigned to the user</param>
    protected virtual SearchOptions CreateDefaultOptions(AcmeSearchQuery request, List<string> rolesTheUserIsAssigned)
    {
        string filter = FieldService.BuildODataFilter(request.Filters, rolesTheUserIsAssigned);
        int skip = (request.PageNumber - 1) * request.ItemsPerPage;

        var options = new SearchOptions
        {
            Filter = filter,
            HighlightPreTag = "<b>",
            HighlightPostTag = "</b>",
            IncludeTotalCount = request.IncludeCount,
            OrderBy = { "search.score() desc" },
            SearchMode = request.IncludeAllWords ? SearchMode.All : SearchMode.Any,
            Skip = skip < 1 ? (int?)null : skip,
            Size = request.ItemsPerPage,
        };

        FieldService.AddFacets(options);

        FieldService.AddHighlightFields(options);

        if (request.OrderByFields.Count == 0)
            FieldService.AddScoreToOrderBy(options);
        else FieldService.AddOrderBy(options, request.OrderByFields);


        return options;
    }

    /// <summary>Maps the highlight fields onto the document object.  This is handy if you don't really need the original document text
    /// and just want highlight text returned in the document object.</summary>
    /// <param name="docs">The docs that were found by the Azure Search method.</param>
    /// <param name="dividerBetweenHighlights">The text you would like between the highlights if more than one is found for a given field.
    /// In a field that contains a LOT of text, it's possible that you could get more than one highlight in different parts of the document.
    /// Sometimes it looks like it breaks it up by sentences and sometimes not.  This is what you want between the highlights.  It may not
    /// make any sense if the user sees them all smashed together so two breaks is the default.</param>
    /// <exception cref="ArgumentException">If you give us a property name that doesn't exist, you could get an exception.</exception>
    protected virtual void MapHighlightsOnToDocument(List<SearchResult<TIndexClass>> docs, string dividerBetweenHighlights = "<br/><br/>")
    {
        var sb = new StringBuilder();

        foreach (SearchResult<TIndexClass> oneDoc in docs)
        {
            if (oneDoc.Highlights == null) continue;
            
            foreach (var oneDocHighlight in oneDoc.Highlights)
            {
                if (oneDocHighlight.Value == null || oneDocHighlight.Value.Count == 0) continue;

                var searchField = FieldService.FindByIndexFieldName(oneDocHighlight.Key);

                if (searchField == null)
                    throw new ArgumentException($"The field service could not find a property on the {typeof(TIndexClass)} named {oneDocHighlight.Key}!");
                
                var prop = typeof(TIndexClass).GetProperty(searchField.PropertyFieldName);
                if (prop == null) throw new ArgumentException($"There is no property on the {typeof(TIndexClass)} named {searchField.PropertyFieldName}!");
                
                // You'll have more than one string if the text property being search is very large.
                // The individual strings will correspond to sentences most of the time, but not always.
                sb.Clear();
                foreach (string oneHighlight in oneDocHighlight.Value)
                {
                    if (sb.Length > 0)
                        sb.Append(dividerBetweenHighlights);
                    sb.Append(oneHighlight);
                }

                prop.SetValue(oneDoc.Document, sb.ToString());
            }
        }

    }

}