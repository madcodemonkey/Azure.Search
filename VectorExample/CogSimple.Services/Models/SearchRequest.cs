using Azure.Search.Documents.Models;

namespace CogSimple.Services;

public class SearchRequest
{
    public IList<string>? DocumentFields { get; set; }
    public bool IncludeAllWords { get; set; } = true;
    public bool IncludeCount { get; set; } = true;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string Query { get; set; } = "*";
    public SearchQueryType QueryType { get; set; } = SearchQueryType.Simple;
    public IList<string>? SearchFields { get; set; }
    public IList<string>? VectorFields { get; set; }
    public bool VectorOnlySearch { get; set; }
}