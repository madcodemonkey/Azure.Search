namespace Search.Model;

public class AcmeSearchQueryResult<T> : AcmeSearchQuery where T : class 
{
    public AcmeSearchQueryResult() : base()
    {
        Docs = new List<T>();
        Facets = new List<AcmeSearchFacet>();
        Diagnostics = new AcmeSearchQueryDiagnostics();
    }
    public List<T> Docs { get; set; }
    public List<AcmeSearchFacet> Facets { get; set; }
    public long TotalCount { get; set; }
    public AcmeSearchQueryDiagnostics Diagnostics { get; set; }
}