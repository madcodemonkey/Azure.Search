using Azure.Search.Documents.Models;

namespace CogSearchServices.Services;

public static class SearchResultsExtensions
{
    public static async Task<List<SearchResult<T>>> ToSearchResultDocumentsAsync<T>(this SearchResults<T> azSearchResults)
    {
        var result = new List<SearchResult<T>>();
        
        await foreach (SearchResult<T>? item in azSearchResults.GetResultsAsync())
        {
            result.Add(item);
        }

        return result;
    }
}