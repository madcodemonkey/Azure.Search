namespace Search.CogServices;

public class AcmeSearchQueryResultV2<T>  
{
    public AcmeSearchQueryResultV2() : base()
    {
        Docs = new List<T>();
        Facets = new List<AcmeSearchFacet>();
    }


    /// <summary>Number of items to show per page.</summary>
    public int ItemsPerPage { get; set; }

    /// <summary>Current page number</summary>
    public int PageNumber { get; set; }


    public List<T> Docs { get; set; }
    public List<AcmeSearchFacet> Facets { get; set; }
    public long TotalCount { get; set; }
}