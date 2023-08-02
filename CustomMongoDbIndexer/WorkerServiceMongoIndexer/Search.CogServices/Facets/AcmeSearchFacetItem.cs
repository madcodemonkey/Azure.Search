namespace Search.CogServices;

public class AcmeSearchFacetItem
{
    /// <summary>How many times was this found?</summary>
    public long Count { get; set; }

    /// <summary>A facet is selected if it is part of the filter.</summary>
    public bool Selected { get; set; }

    /// <summary>Text describing the facet.</summary>
    public string Text { get; set; } = string.Empty;
}