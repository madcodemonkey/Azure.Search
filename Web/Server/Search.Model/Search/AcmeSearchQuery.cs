namespace Search.Model;

public class AcmeSearchQuery 
{
    public AcmeSearchQuery()
    {
        Filters = new List<AcmeSearchFilterItem>();
    }

    /// <summary>The simple or Lucene style query</summary>
    public string Query { get; set; }

    /// <summary>There really are only two options.  All or Any.  
    /// The default is Any (false is this case which is also the default value of a boolean)</summary>
    public bool IncludeAllWords { get; set; }

    /// <summary>Filters to narrow the search.  This help with response time a lot.</summary>
    /// <remarks>I'm not letting the user build them because filters are also part of security</remarks>
    public List<AcmeSearchFilterItem> Filters { get; set; }

    /// <summary>Including the count requires more horse power from Azure Search and will take longer, but it is necessary for pagination.</summary>
    public bool IncludeCount { get; set; }

    /// <summary>Number of items to show per page.</summary>
    public int ItemsPerPage { get; set; }

    /// <summary>Current page number</summary>
    public int PageNumber { get; set; }
}