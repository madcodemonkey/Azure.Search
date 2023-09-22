using Azure.Search.Documents.Indexes;

namespace OutOfTheBoxGremlinIndexer.Models;

public class SearchIndexDocument
{
    [SearchableField(IsKey = true, IsFilterable = true)]
    public string Id { get; set; } = string.Empty;

    [SimpleField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
    public int? Age { get; set; }

    [SearchableField(IsFilterable = true, IsSortable = true)]
    public string FirstName { get; set; } = string.Empty;
    [SearchableField(IsFilterable = true, IsSortable = true)]
    public string LastName { get; set; } = string.Empty;

    [SearchableField(IsFilterable = true, IsFacetable = true, IsSortable = true)]
    public string Label { get; set; } = string.Empty;

    [SearchableField]
    public string Rid { get; set; } = string.Empty;
}