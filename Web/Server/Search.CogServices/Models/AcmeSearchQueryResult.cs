namespace Search.CogServices;

public class AcmeSearchQueryResult<T> : AcmeSearchQuery where T : class 
{
    public AcmeSearchQueryResult() : base()
    {
        Docs = new List<T>();
        Facets = new List<AcmeSearchFacet>();
    }
    public List<T> Docs { get; set; }
    public List<AcmeSearchFacet> Facets { get; set; }
    public long TotalCount { get; set; }
}