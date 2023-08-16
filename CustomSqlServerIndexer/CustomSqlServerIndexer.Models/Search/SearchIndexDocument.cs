using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;

namespace CustomSqlServerIndexer.Models;

public class SearchIndexDocument
{
    [SimpleField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
    public double? BaseRate { get; set; }

    [SearchableField(IsFilterable = true, IsFacetable = true, IsSortable = true)]
    public string Category { get; set; } = string.Empty;

    [SearchableField]
    public string Description { get; set; } = string.Empty;

    [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.FrLucene)]
    public string DescriptionFr { get; set; } = string.Empty;

    [SearchableField(IsKey = true, IsFilterable = true)]
    public string HotelId { get; set; } = string.Empty;

    [SearchableField(IsFilterable = true, IsSortable = true)]
    public string HotelName { get; set; } = string.Empty;

    [SimpleField(IsFilterable = true, IsFacetable = true)]
    public bool IsDeleted { get; set; }

    [SimpleField(IsFilterable = true, IsSortable = true)]
    public DateTimeOffset? LastRenovationDate { get; set; }
 
    [SimpleField(IsFilterable = true, IsFacetable = true)]
    public bool? ParkingIncluded { get; set; }

    [SimpleField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
    public int? Rating { get; set; }

    [SearchableField(IsFilterable = true)]
    public string[] Roles { get; set; } = Array.Empty<string>();

    [SimpleField(IsFilterable = true, IsFacetable = true)]
    public bool? SmokingAllowed { get; set; }

    [SearchableField(IsFilterable = true, IsFacetable = true)]
    public string[] Tags { get; set; } = Array.Empty<string>();

    public override string ToString()
    {
        string tags = Tags != null ? string.Join(',', Tags) : string.Empty;

        return $"Hotel id: {HotelId} | Name: {HotelName} | Rating: {Rating} | Tags: {tags} | Category: {Category}";
    }
}