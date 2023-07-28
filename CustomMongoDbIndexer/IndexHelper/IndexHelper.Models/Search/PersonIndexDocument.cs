using Azure.Search.Documents.Indexes;

namespace IndexHelper.Models;

public class PersonIndexDocument
{
    [SimpleField(IsKey = true, IsFilterable = true)]
    public string Id { get; set; }

    [SearchableField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
    public string FirstName { get; set; }

    [SearchableField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
    public string LastName { get; set; }

    [SimpleField(IsFilterable = true, IsSortable = true)]
    public int Age { get; set; }

    [SearchableField(IsFilterable = true)]
    public string Description { get; set; }
}