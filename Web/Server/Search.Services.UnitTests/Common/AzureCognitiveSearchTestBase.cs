using Azure;
using Azure.Search.Documents.Models;
using Moq;

namespace Search.Services.UnitTests;

public abstract class AzureCognitiveSearchTestBase
{
    /// <summary>Used to create response search results object when you don't care about highlights.</summary>
    private Response<SearchResults<T>> CreateAzureSearchResponse<T>(List<T> documents)
    {
        var docs = documents.Select(s => new TestSearchDocument<T> { Document = s, Highlights = null }).ToList();

        return CreateAzureSearchResponse(docs);
    }

    /// <summary>Used to create response search results object when you want to specify highlight information.</summary>
    protected Response<SearchResults<T>> CreateAzureSearchResponse<T>(List<TestSearchDocument<T>> documents)
    {
        var mockResponse = new Mock<Response>();

        SearchResults<T> mockResults = CreateAzureSearchResults(documents);

        Response<SearchResults<T>> result = Response.FromValue(mockResults, mockResponse.Object);

        return result;
    }

    /// <summary>Used to create search results object when you want to specify highlight information.</summary>
    protected SearchResults<T> CreateAzureSearchResults<T>(List<TestSearchDocument<T>> documents)
    {
        // Based on https://stackoverflow.com/a/66810865
        var docs = documents.Select(s => SearchModelFactory.SearchResult(s.Document, s.Score, s.Highlights)).ToList();

        var mockResponse = new Mock<Response>();

        var result = SearchModelFactory.SearchResults(docs, documents.Count, null, 0.85, mockResponse.Object);

        return result;
    }

    /// <summary>Used to extract the search result from the async pageable object into a list that we can more easily use.</summary>
    protected async Task<List<SearchResult<T>>> ConvertDocumentsAsync<T>(SearchResults<T> azSearchDocuments)
    {
        var result = new List<SearchResult<T>>();

        AsyncPageable<SearchResult<T>> resultList = azSearchDocuments.GetResultsAsync();

        await foreach (var item in resultList)
        {
            result.Add(item);
        }

        return result;
    }
}