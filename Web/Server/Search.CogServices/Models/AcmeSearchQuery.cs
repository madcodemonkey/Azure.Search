using Azure.Search.Documents.Models;

namespace Search.CogServices;

public class AcmeSearchQuery
{
    /// <summary>Filters to narrow the search.  This help with response time a lot.</summary>
    /// <remarks>I'm not letting the user build them because filters are also part of security</remarks>
    public List<AcmeSearchFilterField> Filters { get; set; } = new List<AcmeSearchFilterField>();

    /// <summary>There really are only two options.  All or Any.
    /// The default is Any (false is this case which is also the default value of a boolean)</summary>
    public bool IncludeAllWords { get; set; }

    /// <summary>Including the count requires more horse power from Azure Search and will take longer, but it is necessary for pagination.</summary>
    public bool IncludeCount { get; set; }

    /// <summary>Number of items to show per page.</summary>
    public int ItemsPerPage { get; set; }

    /// <summary>A list of fields that you want to use to order the results.  You can have more than one order by
    /// and the order they appear in this list matters!!  If nothing is specified, we will sort by Score order
    /// descending order (highest to lowest score)</summary>
    public List<AcmeSearchOrderBy> OrderByFields { get; set; } = new List<AcmeSearchOrderBy>();

    /// <summary>Current page number</summary>
    public int PageNumber { get; set; }

    /// <summary>The simple or Lucene style query</summary>
    public string Query { get; set; }

    /// <summary> A value that specifies the syntax of the search query. The default is simple Use full; if your query uses the Lucene query syntax. </summary>
    /// <remarks>You need a beta version of the search NuGet package to get semantic search as an option!</remarks>
    public SearchQueryType QueryType { get; set; } = SearchQueryType.Simple;
}