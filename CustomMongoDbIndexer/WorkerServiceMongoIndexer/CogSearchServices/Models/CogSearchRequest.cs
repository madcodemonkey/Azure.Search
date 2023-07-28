using Azure.Search.Documents.Models;

namespace CogSearchServices.Models;

public class CogSearchRequest
{
    public bool IncludeAllWords { get; set; } = true;
    public bool IncludeCount { get; set; } = true;
    public string Query { get; set; } = "*";
    public SearchQueryType QueryType { get; set; } = SearchQueryType.Simple;
}