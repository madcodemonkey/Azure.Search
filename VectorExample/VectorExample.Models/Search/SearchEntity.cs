using Azure.Search.Documents.Indexes;

namespace VectorExample.Models;

public class SearchEntity
{
    [SearchableField(IsFilterable = true, IsFacetable = true)]
    public string Category { get; set; }

    [SearchableField(IsFilterable = true, IsFacetable = true)]
    public string Subcategory { get; set; }

    public string Text { get; set; }
}