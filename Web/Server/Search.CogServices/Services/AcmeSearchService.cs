using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;

namespace Search.CogServices;

public class AcmeSearchService : IAcmeSearchService
{
    private readonly IAcmeODataService _oDataService;
    private readonly IAcmeSearchIndexService _searchIndexService;

    /// <summary>Constructor</summary>
    public AcmeSearchService(IAcmeSearchIndexService searchIndexService,
        IAcmeODataService oDataService)
    {
        _oDataService = oDataService;
        _searchIndexService = searchIndexService;
    }

    /// <summary>The Search index service, which is a wrapper around Microsoft's SearchIndexClient class.</summary>

    /// <summary>Searches using the Azure Search API.</summary>
    /// <param name="request">The request from the user.</param>
    /// <param name="securityTrimmingFieldName">The name of the field (as specified in the Azure Index and it is case sensitive)
    /// being used for security trimming.  It's assumed that it is a string collection.</param>
    /// <param name="securityTrimmingValues">The values that the current user has that we will try to match.  In other words, if they have the 'admin' role,
    /// we will only bring back records that have the 'admin' role on them.</param>
    public virtual async Task<AcmeSearchQueryResult<SearchResult<SearchDocument>>> SearchAsync(AcmeSearchQuery request,
        string? securityTrimmingFieldName = null, List<string?>? securityTrimmingValues = null)
    {
        SearchOptions options = CreateDefaultSearchOptions(request, securityTrimmingFieldName, securityTrimmingValues);

        return await SearchAsync(request, options);
    }

    /// <summary>Searches using the Azure Search API.</summary>
    /// <param name="request">The request from the user.</param>
    /// <param name="options">The search options to use when searching for data in Azure Search.</param>
    public virtual async Task<AcmeSearchQueryResult<SearchResult<SearchDocument>>> SearchAsync(AcmeSearchQuery request, SearchOptions options)
    {
        if (string.IsNullOrWhiteSpace(request.IndexName))
            throw new ArgumentNullException(request.IndexName, "You must specify the name of the index to search!");

        var azSearchResult = await _searchIndexService.SearchAsync<SearchDocument>(request.IndexName, request.Query, options);

        var result = await WrapResultsAsync(request, azSearchResult);

        return result;
    }

    /// <summary>Searches using the Azure Search API.</summary>
    /// <param name="request">The request from the user.</param>
    /// <param name="configurationName">The name of the semantic configuration in the Azure Portal that should be used.</param>
    /// <param name="securityTrimmingFieldName">The name of the field (as specified in the Azure Index and it is case sensitive)
    /// being used for security trimming.  It's assumed that it is a string collection.</param>
    /// <param name="securityTrimmingValues">The values that the current user has that we will try to match.  In other words, if they have the 'admin' role,
    /// we will only bring back records that have the 'admin' role on them.</param>
    public virtual async Task<AcmeSearchQueryResult<SearchResult<SearchDocument>>> SemanticSearchAsync(AcmeSearchQuery request, string configurationName,
        string? securityTrimmingFieldName = null, List<string?>? securityTrimmingValues = null)
    {
        SearchOptions options = CreateSemanticSearchOptions(request, configurationName, securityTrimmingFieldName, securityTrimmingValues);

        return await SearchAsync(request, options);
    }

    /// <summary>Creates a set of default options you can then override if necessary.</summary>
    /// <param name="request">The request from the user.</param>
    /// <param name="securityTrimmingFieldName">The name of the field (as specified in the Azure Index and it is case sensitive)
    /// being used for security trimming.  It's assumed that it is a string collection.</param>
    /// <param name="securityTrimmingValues">The values that the current user has that we will try to match.  In other words, if they have the 'admin' role,
    /// we will only bring back records that have the 'admin' role on them.</param>
    protected virtual SearchOptions CreateDefaultSearchOptions(AcmeSearchQuery request,
        string? securityTrimmingFieldName = null, List<string?>? securityTrimmingValues = null)
    {
        string filter = _oDataService.BuildODataFilter(request.IndexName, request.Filters, securityTrimmingFieldName, securityTrimmingValues);
        int skip = (request.PageNumber - 1) * request.ItemsPerPage;

        var options = new SearchOptions
        {
            Filter = filter,
            HighlightPreTag = request.HighlightPreTag,
            HighlightPostTag = request.HighlightPostTag,
            IncludeTotalCount = request.IncludeCount,
            QueryType = request.QueryType,
            SearchMode = request.IncludeAllWords ? SearchMode.All : SearchMode.Any,
            Size = request.ItemsPerPage,
            Skip = skip < 1 ? (int?)null : skip
        };

        if (request.FacetFields != null)
        {
            foreach (string fieldName in request.FacetFields)
            {
                options.Facets.Add(fieldName);
            }
        }

        if (request.HighlightFields != null)
        {
            foreach (string fieldName in request.HighlightFields)
            {
                options.HighlightFields.Add(fieldName);
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

        return options;
    }
    
    /// <summary>Creates a search options object for semantic search.  Semantic search options can't contain
    /// certain things and need others that are different form a normal search.</summary>
    /// <param name="request">The request from the user.</param>
    /// <param name="configurationName">The name of the semantic configuration in the Azure Portal that should be used.</param>
    /// <param name="securityTrimmingFieldName">The name of the field (as specified in the Azure Index and it is case sensitive)
    /// being used for security trimming.  It's assumed that it is a string collection.</param>
    /// <param name="securityTrimmingValues">The values that the current user has that we will try to match.  In other words, if they have the 'admin' role,
    /// we will only bring back records that have the 'admin' role on them.</param>
    protected virtual SearchOptions CreateSemanticSearchOptions(AcmeSearchQuery request, string configurationName,
        string? securityTrimmingFieldName = null, List<string?>? securityTrimmingValues = null)
    {
        string filter = _oDataService.BuildODataFilter(request.IndexName, request.Filters, securityTrimmingFieldName, securityTrimmingValues);
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
    protected virtual async Task<List<SearchResult<SearchDocument>>> GetPagedResultsAsync(SearchResults<SearchDocument> azSearchResults)
    {
        var result = new List<SearchResult<SearchDocument>>();

        AsyncPageable<SearchResult<SearchDocument>> azOnePageOfSearchDocuments = azSearchResults.GetResultsAsync();

        await foreach (SearchResult<SearchDocument> item in azOnePageOfSearchDocuments)
        {
            result.Add(item);
        }

        return result;
    }

    /// <summary>Wraps the search results that came back from the Azure Search Index in a AcmeSearchQueryResult instance.
    /// You will still get the raw result, but with additional information that you can return the the client.</summary>
    /// <param name="request">The search request from the client side.</param>
    /// <param name="azSearchResult">The response from the Azure Search index.</param>
    protected virtual async Task<AcmeSearchQueryResult<SearchResult<SearchDocument>>> WrapResultsAsync(
        AcmeSearchQuery request, Response<SearchResults<SearchDocument>> azSearchResult)
    {
        var result = new AcmeSearchQueryResult<SearchResult<SearchDocument>>
        {
            Docs = await GetPagedResultsAsync(azSearchResult.Value),
            Facets = ConvertFacets(azSearchResult.Value.Facets, request.Filters),
            ItemsPerPage = request.ItemsPerPage,
            PageNumber = request.PageNumber,
            TotalCount = azSearchResult.Value.TotalCount ?? 0
        };

        return result;
    }

    /// <summary>Avoiding a leaky abstraction by converting Azure Search facets to our format.
    /// Given a list of facets from Azure Search compare them to the filter list we are using and
    /// mark them as "Selected" so that the user knows they are being used to filter results</summary>
    /// <param name="facets">Facets from an Azure Search call</param>
    /// <param name="fieldFilters">A list of field filter where each represents a grouping of filters for one field.</param>
    /// <returns></returns>
    private List<AcmeSearchFacet> ConvertFacets(IDictionary<string, IList<FacetResult>>? facets, List<AcmeSearchFilterFieldV2>? fieldFilters)
    {
        var result = new List<AcmeSearchFacet>();

        if (facets == null)
            return result;

        foreach (KeyValuePair<string, IList<FacetResult>> facet in facets)
        {
            var oneFacet = new AcmeSearchFacet();

            string facetName = facet.Key;
          
            oneFacet.FieldName = facetName;
            oneFacet.DisplayText = facetName; // searchFilter.DisplayName;   // TODO: Figure out how to do the display name by splitting on camel case values.

            foreach (FacetResult item in facet.Value)
            {
                var text = item.Value.ToString();

                oneFacet.Items.Add(new AcmeSearchFacetItem
                {
                    Text = text,
                    Count = item.Count ?? 0,
                    Selected = IsFacetSelected(fieldFilters, facetName, text)
                });
            }

            result.Add(oneFacet);
        }

        return result;
    }

    /// <summary>Indicates if the facet should be selected or not.</summary>
    /// <param name="fieldFilters">A list of field filter where each represents a grouping of filters for one field.</param>
    /// <param name="fieldName">The name of the filter to examine for the values</param>
    /// <param name="facetText">The text of the facet item</param>
    private bool IsFacetSelected(List<AcmeSearchFilterFieldV2>? fieldFilters, string fieldName, string facetText)
    {
        if (fieldFilters == null) return false;

        var group = fieldFilters.FirstOrDefault(w => w.FieldName == fieldName);
        if (group == null) return false;

        return group.Filters.Any(w => string.Compare(w.Values[0], facetText, StringComparison.InvariantCultureIgnoreCase) == 0);
    }

}