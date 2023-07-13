using Azure.Search.Documents.Indexes;

namespace CustomBlobIndexer.Models;

public class Entity
{
    [SearchableField(IsFilterable = true, IsFacetable = true)]
    public string Category { get; set; }
    [SearchableField(IsFilterable = true, IsFacetable = true)]
    public string Subcategory { get; set; }
    public string Text { get; set; }
}