namespace Search.CogServices;

public class AcmeSearchFacet
{
    /// <summary>What to display on the screen</summary>
    public string DisplayText { get; set; } = string.Empty;

    /// <summary>
    /// The name of the field associated with this facet.
    /// </summary>
    public string FieldName { get; set; } = string.Empty;

    public List<AcmeSearchFacetItem> Items { get; set; } = new List<AcmeSearchFacetItem>();
}