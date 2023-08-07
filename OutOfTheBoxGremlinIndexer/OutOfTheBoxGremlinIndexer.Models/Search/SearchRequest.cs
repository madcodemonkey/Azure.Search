using Azure.Search.Documents.Models;

namespace CustomSqlServerIndexer.Models;

public class SearchRequest
{
    public bool IncludeAllWords { get; set; } = true;
    public bool IncludeCount { get; set; } = true;
    public string Query { get; set; } = "*";
    public int PageSize { get; set; } = 10;
    public int PageNumber { get; set; } = 1;
    public SearchQueryType QueryType { get; set; } = SearchQueryType.Simple;
}