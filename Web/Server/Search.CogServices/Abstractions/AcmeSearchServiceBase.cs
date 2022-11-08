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
    protected IAcmeSearchIndexService SearchIndexService { get; }
    protected IAcmeFieldService FieldService { get; }
    protected abstract string IndexName { get; }

    /// <summary>Searches using the Azure Search API.</summary>
    /// <param name="request">The request from the user.</param>
    /// <param name="rolesTheUserIsAssigned">The roles assigned to the user</param>
    public async Task<AcmeSearchQueryResult<SearchResult<TIndexClass>>> SearchAsync(AcmeSearchQuery request, List<string> rolesTheUserIsAssigned)
    {
        SearchOptions options = CreateDefaultOptions(request, rolesTheUserIsAssigned);

        var azSearchResult = await SearchIndexService.SearchAsync<TIndexClass>(IndexName, request.Query, options);

        var result = await ConvertResultsAsync(request, azSearchResult);

        return result;
    }

    
    /// <summary>Converts the result that came back from the Azure Search Index.</summary>
    /// <param name="request">The search request from the client side.</param>
    /// <param name="azSearchResult">The response from the Azure Search index.</param>
    /// <returns></returns>
    private async Task<AcmeSearchQueryResult<SearchResult<TIndexClass>>> ConvertResultsAsync(AcmeSearchQuery request, Response<SearchResults<TIndexClass>> azSearchResult)
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
            Docs = await ConvertResultDocumentsAsync(azSearchResult.Value),
        };

        return result;
    }

    /// <summary>Converts one page of documents. This method is called by <see cref="ConvertResultsAsync"/> </summary>
    /// <param name="azSearchResults">The search results from the call to the Azure Search PAI.</param>
    protected virtual async Task<List<SearchResult<TIndexClass>>> ConvertResultDocumentsAsync(SearchResults<TIndexClass> azSearchResults)
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

        if (request.OrderByFields.Count == 0)
            FieldService.AddScoreToOrderBy(options);
        else FieldService.AddOrderBy(options, request.OrderByFields);


        return options;
    }
}