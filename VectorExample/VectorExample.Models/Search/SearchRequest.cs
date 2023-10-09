using Azure.Search.Documents.Models;

namespace VectorExample.Models;

public class SearchRequest
{
    public bool IncludeAllWords { get; set; } = true;
    public bool IncludeCount { get; set; } = true;
    public string Query { get; set; } = "*";
    public SearchQueryType QueryType { get; set; } = SearchQueryType.Simple;
}