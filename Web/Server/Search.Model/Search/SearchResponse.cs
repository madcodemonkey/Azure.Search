namespace Search.Model;

public class SearchResponse<T> where T : class
{
    public long TotalCount { get; set; }
    public int PageNumber { get; set; }
    public List<SearchFacetResponse> Facets { get; set; } = new List<SearchFacetResponse>();
    public List<T> Data { get; set; } = new List<T>();
}