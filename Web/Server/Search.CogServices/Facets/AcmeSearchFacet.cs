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

    /// <summary>
    /// The name of the field associated with this facet.
    /// </summary>
    public string FieldName { get; set; }

    public List<AcmeSearchFacetItem> Items { get; set; }
}