namespace Search.Model;

public class AcmeSearchFacet
{
    public AcmeSearchFacet()
    {
        Items = new List<AcmeSearchFacetItem>();
    }

    /// <summary>Facet id.</summary>
    public int Id { get; set; }

    /// <summary>What to display on the screen</summary>
    public string DisplayText { get; set; }

    public List<AcmeSearchFacetItem> Items { get; set; }
}