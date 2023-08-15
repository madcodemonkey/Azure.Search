using Azure.Search.Documents.Indexes;

namespace OutOfTheBoxBlobIndexer.Models;

public class SearchEntity
{
    [SearchableField(IsFilterable = true, IsFacetable = true)]
    public string Category { get; set; } = string.Empty;

    [SearchableField(IsFilterable = true, IsFacetable = true)]
    public string Subcategory { get; set; } = string.Empty;

    public string Text { get; set; } = string.Empty;
}