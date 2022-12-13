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

    /// <summary>A service to help us handle fields.  It also adds facets and helps build filters.</summary>
    protected IAcmeFieldService FieldService { get; }

    /// <summary>The name of the index we are querying.</summary>
    protected abstract string IndexName { get; }

    /// <summary>The Search index service, which is a wrapper around Microsoft's SearchIndexClient class.</summary>
    protected IAcmeSearchIndexService SearchIndexService { get; }

    /// <summary>Searches using the Azure Search API.</summary>
    /// <param name="request">The request from the user.</param>
    /// <param name="rolesTheUserIsAssigned">Case sensitive list of roles that for search trimming.</param>
    public virtual async Task<AcmeSearchQueryResult<SearchResult<TIndexClass>>> SearchAsync(AcmeSearchQuery request, List<string?> rolesTheUserIsAssigned)
    {
        SearchOptions options = CreateDefaultSearchOptions(request, rolesTheUserIsAssigned);

        return await SearchAsync(request, options);
    }

    /// <summary>Searches using the Azure Search API.</summary>
    /// <param name="request">The request from the user.</param>
    /// <param name="options">The search options to use when searching for data in Azure Search.</param>
    public virtual async Task<AcmeSearchQueryResult<SearchResult<TIndexClass>>> SearchAsync(AcmeSearchQuery request, SearchOptions options)
    {
        if (string.IsNullOrWhiteSpace(IndexName))
            throw new ArgumentNullException(IndexName, "You must specify a value for the IndexName property in order to search an index!");

        var azSearchResult = await SearchIndexService.SearchAsync<TIndexClass>(IndexName, request.Query, options);

        var result = await WrapResultsAsync(request, azSearchResult);

        return result;
    }

    /// <summary>Searches using the Azure Search API.</summary>
    /// <param name="request">The request from the user.</param>
    /// <param name="rolesTheUserIsAssigned">Case sensitive list of roles that for search trimming.</param>
    /// <param name="configurationName">The name of the semantic configuration in the Azure Portal that should be used.</param>
    public virtual async Task<AcmeSearchQueryResult<SearchResult<TIndexClass>>> SemanticSearchAsync(AcmeSearchQuery request,
        List<string?> rolesTheUserIsAssigned, string configurationName)
    {
        SearchOptions options = CreateSemanticSearchOptions(request, rolesTheUserIsAssigned, configurationName);

        return await SearchAsync(request, options);
    }

    /// <summary>Creates a set of default options you can then override if necessary.</summary>
    /// <param name="request">The request from the user.</param>
    /// <param name="rolesTheUserIsAssigned">The roles assigned to the user</param>
    protected virtual SearchOptions CreateDefaultSearchOptions(AcmeSearchQuery request, List<string?> rolesTheUserIsAssigned)
    {
        string filter = FieldService.BuildODataFilter(request.Filters, rolesTheUserIsAssigned);
        int skip = (request.PageNumber - 1) * request.ItemsPerPage;

        var options = new SearchOptions
        {
            Filter = filter,
            HighlightPreTag = "<b>",
            HighlightPostTag = "</b>",
            IncludeTotalCount = request.IncludeCount,
            QueryType = request.QueryType,
            SearchMode = request.IncludeAllWords ? SearchMode.All : SearchMode.Any,
            Size = request.ItemsPerPage,
            Skip = skip < 1 ? (int?)null : skip
        };

        FieldService.AddFacets(options);

        FieldService.AddHighlightFields(options);

        if (request.OrderByFields.Count == 0)
            FieldService.AddScoreToOrderBy(options);
        else FieldService.AddOrderBy(options, request.OrderByFields);

        return options;
    }

    /// <summary>Creates a search options object for semantic search.  Semantic search options can't contain
    /// certain things and need others that are different form a normal search.</summary>
    /// <param name="request">The request from the user.</param>
    /// <param name="rolesTheUserIsAssigned">The roles assigned to the user</param>
    /// <param name="configurationName">The name of the semantic configuration in the Azure Portal that should be used.</param>
    protected virtual SearchOptions CreateSemanticSearchOptions(AcmeSearchQuery request,
        List<string?> rolesTheUserIsAssigned, string configurationName)
    {
        string filter = FieldService.BuildODataFilter(request.Filters, rolesTheUserIsAssigned);
        int skip = (request.PageNumber - 1) * request.ItemsPerPage;

        var options = new SearchOptions
        {
            Filter = filter,
            //HighlightPreTag = "<b>",
            //HighlightPostTag = "</b>",
            IncludeTotalCount = request.IncludeCount,
            QueryType = SearchQueryType.Semantic,
            SearchMode = null, // setting this to SearchMode.All or SearchMode.Any causes zero records to be returned!
            Skip = skip < 1 ? (int?)null : skip,
            Size = request.ItemsPerPage
        };

        // Warning 1: 'orderBy' is not supported when 'queryType' is set to 'semantic'.
        options.OrderBy.Clear();

        // Warning 2:  'queryLanguage' is required when 'speller' is specified or 'queryType' is set to 'semantic'
        options.QueryLanguage ??= QueryLanguage.EnUs;
        options.SemanticConfigurationName = configurationName;

        return options;
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

    /// <summary>Wraps the search results that came back from the Azure Search Index in a AcmeSearchQueryResult instance.
    /// You will still get the raw result, but with additional information that you can return the the client.</summary>
    /// <param name="request">The search request from the client side.</param>
    /// <param name="azSearchResult">The response from the Azure Search index.</param>
    protected virtual async Task<AcmeSearchQueryResult<SearchResult<TIndexClass>>> WrapResultsAsync(AcmeSearchQuery request, Response<SearchResults<TIndexClass>> azSearchResult)
    {
        var result = new AcmeSearchQueryResult<SearchResult<TIndexClass>>
        {
            Docs = await GetPagedResultsAsync(azSearchResult.Value),
            Facets = FieldService.ConvertFacets(azSearchResult.Value.Facets, request.Filters),
            Filters = request.Filters,
            IncludeAllWords = request.IncludeAllWords,
            IncludeCount = request.IncludeCount,
            ItemsPerPage = request.ItemsPerPage,
            Query = request.Query,
            QueryType = request.QueryType,
            OrderByFields = request.OrderByFields,
            PageNumber = request.PageNumber,
            TotalCount = azSearchResult.Value.TotalCount ?? 0
        };

        return result;
    }
}