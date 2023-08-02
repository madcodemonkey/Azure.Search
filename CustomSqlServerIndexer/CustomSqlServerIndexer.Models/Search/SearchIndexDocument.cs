using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using Microsoft.Spatial;
using System.Text.Json.Serialization;

namespace CustomSqlServerIndexer.Models;

public class SearchIndexDocument
{
    [SimpleField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
    [JsonPropertyName("baseRate")]
    public double? BaseRate { get; set; }

    [SearchableField(IsFilterable = true, IsFacetable = true, IsSortable = true)]
    [JsonPropertyName("category")]
    public string Category { get; set; } = string.Empty;

    [SearchableField]
    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.FrLucene)]
    [JsonPropertyName("description_fr")]
    public string DescriptionFr { get; set; } = string.Empty;

    [SearchableField(IsKey = true, IsFilterable = true)]
    [JsonPropertyName("hotelId")]
    public string HotelId { get; set; } = string.Empty;

    [SearchableField(IsFilterable = true, IsSortable = true)]
    [JsonPropertyName("hotelName")]
    public string HotelName { get; set; } = string.Empty;

    [SimpleField(IsFilterable = true, IsFacetable = true)]
    [JsonPropertyName("isDeleted")]
    public bool IsDeleted { get; set; }

    [SimpleField(IsFilterable = true, IsSortable = true)]
    [JsonPropertyName("lastRenovationDate")]
    public DateTimeOffset? LastRenovationDate { get; set; }

    /// <summary>Location</summary>
    /// <remarks>Requires Microsoft.Azure.Core.Spatial NuGet package for GeographyPoint</remarks>
    [SimpleField(IsFilterable = true, IsSortable = true)]
    //  [FieldBuilderIgnore]
    [JsonPropertyName("location")]
    public GeographyPoint? Location { get; set; }

    [SimpleField(IsFilterable = true, IsFacetable = true)]
    [JsonPropertyName("parkingIncluded")]
    public bool? ParkingIncluded { get; set; }

    [SimpleField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
    [JsonPropertyName("rating")]
    public int? Rating { get; set; }

    [SearchableField(IsFilterable = true)]
    [JsonPropertyName("roles")]
    public string[] Roles { get; set; } = Array.Empty<string>();

    [SimpleField(IsFilterable = true, IsFacetable = true)]
    [JsonPropertyName("smokingAllowed")]
    public bool? SmokingAllowed { get; set; }

    [SearchableField(IsFilterable = true, IsFacetable = true)]
    [JsonPropertyName("tags")]
    public string[] Tags { get; set; } = Array.Empty<string>();

    public override string ToString()
    {
        string tags = Tags != null ? string.Join(',', Tags) : string.Empty;

        return $"Hotel id: {HotelId} | Name: {HotelName} | Rating: {Rating} | Tags: {tags} | Category: {Category}";
    }
}