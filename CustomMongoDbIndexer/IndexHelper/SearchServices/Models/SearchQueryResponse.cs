using Azure.Search.Documents.Models;

namespace SearchServices.Models;

public class SearchQueryResponse<T> where T : class
{
    public List<SearchResult<T>> Docs { get; set; } = new List<SearchResult<T>>();
    public long? TotalCount { get; set; }
}