using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using Search.Model;

namespace Search.Services;

public class HotelSearchService : IHotelSearchService
{
    private readonly SearchServiceSettings _settings;
    private readonly IAcmeSearchIndexService _acmeSearchIndexService;
    private readonly IHotelFilterService _hotelFilterService;

    /// <summary>Constructor</summary>
    public HotelSearchService(SearchServiceSettings settings, IAcmeSearchIndexService acmeSearchIndexService, IHotelFilterService hotelFilterService)
    {
        _settings = settings;
        _acmeSearchIndexService = acmeSearchIndexService;
        _hotelFilterService = hotelFilterService;
    }
    
    /// <summary>Returns all hotels</summary>
    /// <param name="numberOfItemsPerPage">The total number of items per page</param>
    /// <param name="pageNumber">Current page number</param>
    public async Task<SearchResponse<SearchHotel>> GetAllHotelsAsync(int numberOfItemsPerPage, int pageNumber)
    {
        var options = CreateDefaultSearchOptions(numberOfItemsPerPage, pageNumber);

        // SEARCH!!!!!
        SearchResults<SearchHotel> searchResults = await _acmeSearchIndexService.Search<SearchHotel>(_settings.HotelIndexName, "*", options);

        return await ConvertSearchResultsAsync(pageNumber, searchResults);
    }

    /// <summary>Suggest</summary>
    /// <param name="request">A request for a suggestion</param>
    /// <param name="rolesTheUserIsAssigned">Case sensitive list of roles that for search trimming.</param>
    /// <returns>List of suggestions</returns>
    public async Task<List<string>> SuggestAsync(AcmeSearchQuery request, string[] rolesTheUserIsAssigned)
    {
        //var parameters = new SuggestParameters
        //{                
        //    Filter = _filterService.BuildODataFilter(request.Filters, rolesTheUserIsAssigned),
        //    UseFuzzyMatching = false, // false for performance reasons
        //    // OrderBy = new List<string> { _suggesterFieldName }, // Field must be sortable and you probably want ranking sort anyway!
        //    Select = new List<string> { nameof(SearchHotel.HotelName).ConvertToCamelCase() },
        //    Top = 10
        //};

        var options = new SuggestOptions
        {
            Filter = _hotelFilterService.BuildODataFilter(request.Filters, rolesTheUserIsAssigned),
            HighlightPreTag = "<b>",
            HighlightPostTag = "</b>",
            MinimumCoverage = 33.3,
            OrderBy = { nameof(SearchHotel.HotelName).ConvertToCamelCase() },
            //SearchFields = {  },
            Select = { nameof(SearchHotel.HotelName).ConvertToCamelCase() },
            Size = 10,
            UseFuzzyMatching = false // false for performance reasons
        };

        var suggestions = await _acmeSearchIndexService.SuggestAsync<SearchHotel>(_settings.HotelIndexName, request.Query, _settings.HotelSuggestorName, options);

        return suggestions.Results.Select(s => s.Text).ToList();
    }

    public async Task<AcmeSearchQueryResult<SearchHotel>> SearchAsync(AcmeSearchQuery request, string[] rolesTheUserIsAssigned)
    {
        SearchOptions parameters = BuildParameters(request, rolesTheUserIsAssigned);

        var azSearchResult = await _acmeSearchIndexService.SearchAsync<SearchHotel>(_settings.HotelIndexName, request.Query, parameters);

        var result = new AcmeSearchQueryResult<SearchHotel>
        {
            Query = request.Query,
            Filters = request.Filters,
            Facets = _hotelFilterService.ConvertFacets(azSearchResult.Value.Facets, request.Filters),
            IncludeAllWords = request.IncludeAllWords,
            IncludeCount = request.IncludeCount,
            TotalCount = azSearchResult.Value.TotalCount ?? 0,
            ItemsPerPage = request.ItemsPerPage,
            PageNumber = request.PageNumber,
            Docs = await ConvertDocumentsAsync(azSearchResult.Value),
        };
               
        result.Diagnostics.Query = request.Query;
        result.Diagnostics.Filter = parameters.Filter;

        return result;
    }

    private async Task<List<SearchHotel>> ConvertDocumentsAsync(SearchResults<SearchHotel> searchResults)
    {
        var result = new List<SearchHotel>();

        AsyncPageable<SearchResult<SearchHotel>> resultList = searchResults.GetResultsAsync();

        await foreach (SearchResult<SearchHotel> item in resultList)
        {
            result.Add(item.Document);
            // Console.WriteLine($"Score: {item.Score} - {item.Document}");
        }

        return result;
    }

    private SearchOptions BuildParameters(AcmeSearchQuery request, string[] rolesTheUserIsAssigned)
    {            
        string filter = _hotelFilterService.BuildODataFilter(request.Filters, rolesTheUserIsAssigned);
        int skip = (request.PageNumber - 1) * request.ItemsPerPage;
        
        var parameters = new SearchOptions()
        {
            Filter = filter,
            IncludeTotalCount = request.IncludeCount,
            HighlightPreTag = "<b>",
            HighlightPostTag = "</b>",                
            Skip = skip < 1 ? (int?)null : skip,
            Size = request.ItemsPerPage,
            SearchMode = request.IncludeAllWords ? SearchMode.All : SearchMode.Any
        };

        List<string> facets = _hotelFilterService.BuildFacetList();
        facets.ForEach(s => parameters.Facets.Add(s));

        return parameters;
    }
        
    private List<SearchHotel> ConvertDocuments(IList<SearchResult<SearchHotel>> results)
    {
        return results.Select(item => ConvertDocument(item)).ToList();
    }

    private SearchHotel ConvertDocument(SearchResult<SearchHotel> item)
    {
        //item.Document.Score = item.Score;
        //item.Document.Roles = null; // clear the roles!  Don't send them back to the user!
        return item.Document;
    }

    /// <summary>Returns all hotels</summary>
    /// <param name="numberOfItemsPerPage">The total number of items per page</param>
    /// <param name="pageNumber">Current page number</param>
    /// <param name="minRating">The lowest rating allowed</param>
    public async Task<SearchResponse<SearchHotel>> GetHotelsBasedOnRatingAsync(int numberOfItemsPerPage, int pageNumber, int minRating)
    {
        var options = CreateDefaultSearchOptions(numberOfItemsPerPage, pageNumber);

        var ratingField = nameof(SearchHotel.Rating).ConvertToCamelCase();
        options.Filter = $"{ratingField} gt {minRating}";
        options.OrderBy.Clear();
        options.OrderBy.Add($"{ratingField} desc");

        // SEARCH!!!!!
        SearchResults<SearchHotel> searchResults = await _acmeSearchIndexService.Search<SearchHotel>(_settings.HotelIndexName, "*", options);

        return await ConvertSearchResultsAsync(pageNumber, searchResults);
    }

    /// <summary>Creates some default search options.</summary>
    /// <param name="numberOfItemsPerPage">Number of items on each page (default is ??)</param>
    /// <param name="pageNumber">The page number</param>
    private static SearchOptions CreateDefaultSearchOptions(int numberOfItemsPerPage, int pageNumber)
    {
        var options = new SearchOptions
        {
            IncludeTotalCount = true,
            Filter = "",
            OrderBy = { "search.score() desc" },
            Size = numberOfItemsPerPage,
            Skip = numberOfItemsPerPage * (pageNumber - 1)
        };

        if (options.Skip < 0) options.Skip = 0;


        // Add facets of interest that we want returned.
        options.Facets.Add(nameof(SearchHotel.Category).ConvertToCamelCase());
        options.Facets.Add(nameof(SearchHotel.Tags).ConvertToCamelCase());

        return options;
    }

    private static async Task<SearchResponse<SearchHotel>> ConvertSearchResultsAsync(int pageNumber, SearchResults<SearchHotel> searchResults)
    {
        var result = new SearchResponse<SearchHotel>
        {
            TotalCount = searchResults.TotalCount ?? 0,
            PageNumber = pageNumber
        };

        // Show the facets returned from the search if there are any.  You have to request
        // the fields you want facets on in your search options.  Furthermore, they ONLY include 
        // the values from the records that match the query.  In other words, if your document contained a category field
        // and document A had a category = 'Hilly' and document B had a category of 'Flat' and your search text would ONLY
        // return document A then your category facet would only have 'Hilly' because it will be used as a search filter if chosen.
        if (searchResults.Facets != null && searchResults.Facets.Count > 0)
        {
            foreach (var facet in searchResults.Facets)
            {
                var facetResponse = new SearchFacetResponse
                {
                    Name = facet.Key
                };

                foreach (FacetResult item in facet.Value)
                {
                    facetResponse.Values.Add(item.Value.ToString());
                }

                result.Facets.Add(facetResponse);
            }
        }

        AsyncPageable<SearchResult<SearchHotel>> resultList = searchResults.GetResultsAsync();

        await foreach (SearchResult<SearchHotel> item in resultList)
        {
            result.Data.Add(item.Document);
            // Console.WriteLine($"Score: {item.Score} - {item.Document}");
        }

        return result;
    }
}