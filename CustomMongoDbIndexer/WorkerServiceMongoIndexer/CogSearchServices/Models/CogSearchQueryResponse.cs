using Azure.Search.Documents.Models;

namespace CogSearchServices.Models;

public class CogSearchQueryResponse<T> where T : class
{
    public List<SearchResult<T>> Docs { get; set; } = new List<SearchResult<T>>();
    public long? TotalCount { get; set; }
}