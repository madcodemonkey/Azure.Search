namespace Search.CogServices;

public class AcmeSearchFacet
{
    /// <summary>Constructor</summary>
    public AcmeSearchFacet()
    {
        Items = new List<AcmeSearchFacetItem>();
    }

    /// <summary>What to display on the screen</summary>
    public string DisplayText { get; set; }

    /// <summary>Facet id.</summary>
    public int Id { get; set; }

    public List<AcmeSearchFacetItem> Items { get; set; }
}