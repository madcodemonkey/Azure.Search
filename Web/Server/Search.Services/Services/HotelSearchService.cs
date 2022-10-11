using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using Search.Model;

namespace Search.Services;

public class HotelSearchService : IHotelSearchService
{
    private readonly SearchServiceSettings _settings;
    private readonly ISearchIndexService _searchIndexService;

    /// <summary>Constructor</summary>
    public HotelSearchService(SearchServiceSettings settings, ISearchIndexService searchIndexService)
    {
        _settings = settings;
        _searchIndexService = searchIndexService;
    }
    
    /// <summary>Returns all hotels</summary>
    /// <param name="numberOfItemsPerPage">The total number of items per page</param>
    /// <param name="pageNumber">Current page number</param>
    public async Task<SearchResponse<Hotel>> GetAllHotelsAsync(int numberOfItemsPerPage, int pageNumber)
    {
        var options = CreateDefaultSearchOptions(numberOfItemsPerPage, pageNumber);

        // SEARCH!!!!!
        SearchResults<Hotel> searchResults = await _searchIndexService.Search<Hotel>(_settings.SearchIndexName, "*", options);

        return await ConvertSearchResultsAsync(pageNumber, searchResults);
    }

    /// <summary>Returns all hotels</summary>
    /// <param name="numberOfItemsPerPage">The total number of items per page</param>
    /// <param name="pageNumber">Current page number</param>
    /// <param name="minRating">The lowest rating allowed</param>
    public async Task<SearchResponse<Hotel>> GetHotelsBasedOnRatingAsync(int numberOfItemsPerPage, int pageNumber, int minRating)
    {
        var options = CreateDefaultSearchOptions(numberOfItemsPerPage, pageNumber);

        var ratingField = nameof(Hotel.Rating).ConvertToCamelCase();
        options.Filter = $"{ratingField} gt {minRating}";
        options.OrderBy.Clear();
        options.OrderBy.Add($"{ratingField} desc");

        // SEARCH!!!!!
        SearchResults<Hotel> searchResults = await _searchIndexService.Search<Hotel>(_settings.SearchIndexName, "*", options);

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
        options.Facets.Add(nameof(Hotel.Category).ConvertToCamelCase());
        options.Facets.Add(nameof(Hotel.Tags).ConvertToCamelCase());

        return options;
    }

    private static async Task<SearchResponse<Hotel>> ConvertSearchResultsAsync(int pageNumber, SearchResults<Hotel> searchResults)
    {
        var result = new SearchResponse<Hotel>
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

        AsyncPageable<SearchResult<Hotel>> resultList = searchResults.GetResultsAsync();

        await foreach (SearchResult<Hotel> item in resultList)
        {
            result.Data.Add(item.Document);
            // Console.WriteLine($"Score: {item.Score} - {item.Document}");
        }

        return result;
    }
}