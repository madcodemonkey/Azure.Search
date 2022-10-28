using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;

namespace Search.CogServices;

public abstract class AcmeSearchServiceBase<TResultClass, TIndexClass> where TResultClass : class where TIndexClass : class
{
    /// <summary>Constructor</summary>
    protected AcmeSearchServiceBase(IAcmeSearchIndexService searchIndexService, IAcmeFilterService filterService)
    {
        SearchIndexService = searchIndexService;
        FilterService = filterService;
    }
    protected IAcmeSearchIndexService SearchIndexService { get; }
    protected IAcmeFilterService FilterService { get; }
    protected abstract string IndexName { get; }

    /// <summary>Searches using the Azure Search API.</summary>
    /// <param name="request">The request from the user.</param>
    /// <param name="rolesTheUserIsAssigned">The roles assigned to the user</param>
    public async Task<AcmeSearchQueryResult<TResultClass>> SearchAsync(AcmeSearchQuery request, List<string> rolesTheUserIsAssigned)
    {
        SearchOptions options = CreateDefaultOptions(request, rolesTheUserIsAssigned);

        var azSearchResult = await SearchIndexService.SearchAsync<TIndexClass>(IndexName, request.Query, options);

        var result = await ConvertResultsAsync(request, azSearchResult);

        return result;
    }

    private async Task<AcmeSearchQueryResult<TResultClass>> ConvertResultsAsync(AcmeSearchQuery request, Response<SearchResults<TIndexClass>> azSearchResult)
    {
        var result = new AcmeSearchQueryResult<TResultClass>
        {
            Query = request.Query,
            Filters = request.Filters,
            Facets = FilterService.ConvertFacets(azSearchResult.Value.Facets, request.Filters),
            IncludeAllWords = request.IncludeAllWords,
            IncludeCount = request.IncludeCount,
            TotalCount = azSearchResult.Value.TotalCount ?? 0,
            ItemsPerPage = request.ItemsPerPage,
            PageNumber = request.PageNumber,
            Docs = await ConvertDocumentsAsync(azSearchResult.Value),
        };

        return result;
    }

    /// <summary>Converts one page of documents from a SearchResults to the specified list of TResultClass.
    /// This method is called by ConvertResultsAsync</summary>
    /// <param name="azSearchResults">The search results from the call to the Azure Search PAI.</param>
    protected virtual async Task<List<TResultClass>> ConvertDocumentsAsync(SearchResults<TIndexClass> azSearchResults)
    {
        var result = new List<TResultClass>();

        AsyncPageable<SearchResult<TIndexClass>> azOnePageOfSearchDocuments = azSearchResults.GetResultsAsync();

        await foreach (SearchResult<TIndexClass> item in azOnePageOfSearchDocuments)
        {
            result.Add(ConvertOneDocument(item));
        }

        return result;
    }

    /// <summary>Converts a item (TInputClass) that was found by calling the Azure Search API into
    /// a desired return class (TResultClass).</summary>
    /// <param name="azSearchDocument">The item to convert</param>
    protected abstract TResultClass ConvertOneDocument(SearchResult<TIndexClass> azSearchDocument);

    /// <summary>Creates a set of default options you can then override if necessary.</summary>
    /// <param name="request">The request from the user.</param>
    /// <param name="rolesTheUserIsAssigned">The roles assigned to the user</param>
    protected virtual SearchOptions CreateDefaultOptions(AcmeSearchQuery request, List<string> rolesTheUserIsAssigned)
    {
        string filter = FilterService.BuildODataFilter(request.Filters, rolesTheUserIsAssigned);
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

        FilterService.AddFacets(options);

        return options;
    }
}