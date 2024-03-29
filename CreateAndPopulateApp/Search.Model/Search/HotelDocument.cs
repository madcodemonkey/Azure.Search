﻿using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using Microsoft.Spatial;
using System.Text.Json.Serialization;

namespace Search.Model;

// The JsonPropertyName attribute is defined in the Azure Search .NET SDK.
// Here it used to ensure that Pascal-case property names in the model class are mapped to camel-case
// field names in the index.
public partial class HotelDocument
{
    [SearchableField(IsKey = true, IsFilterable = true)]
    [JsonPropertyName("hotelId")]
    public string HotelId { get; set; }

    [SimpleField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
    [JsonPropertyName("baseRate")]
    public double? BaseRate { get; set; }

    [SearchableField]
    [JsonPropertyName("description")]
    public string Description { get; set; }

    [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.FrLucene)]
    [JsonPropertyName("description_fr")]
    public string DescriptionFr { get; set; }

    [SearchableField(IsFilterable = true, IsSortable = true)]
    [JsonPropertyName("hotelName")]
    public string HotelName { get; set; }

    [SearchableField(IsFilterable = true, IsFacetable = true, IsSortable = true)]
    [JsonPropertyName("category")]
    public string Category { get; set; }

    [SearchableField(IsFilterable = true, IsFacetable = true)]
    [JsonPropertyName("tags")]
    public string[] Tags { get; set; }

    [SimpleField(IsFilterable = true, IsFacetable = true)]
    [JsonPropertyName("parkingIncluded")]
    public bool? ParkingIncluded { get; set; }

    [SimpleField(IsFilterable = true, IsFacetable = true)]
    [JsonPropertyName("smokingAllowed")]
    public bool? SmokingAllowed { get; set; }

    [SimpleField(IsFilterable = true, IsSortable = true)]
    [JsonPropertyName("lastRenovationDate")]
    public DateTimeOffset? LastRenovationDate { get; set; }

    [SimpleField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
    [JsonPropertyName("rating")]
    public int? Rating { get; set; }

    [SearchableField(IsFilterable = true)]
    [JsonPropertyName("roles")]
    public string[] Roles { get; set; }

    [SimpleField(IsFilterable = true, IsFacetable = true)]
    [JsonPropertyName("isDeleted")]
    public bool IsDeleted { get; set; }

    /// <summary>Location</summary>
    /// <remarks>Requires Microsoft.Azure.Core.Spatial NuGet package for GeographyPoint</remarks>
    [SimpleField(IsFilterable = true, IsSortable = true)]
    //  [FieldBuilderIgnore]
    [JsonPropertyName("location")]
    public GeographyPoint Location { get; set; }


    public override string ToString()
    {
        string tags = Tags != null ? string.Join(',', Tags) : string.Empty;

        return $"Hotel id: {HotelId} | Name: {HotelName} | Rating: {Rating} | Tags: {tags} | Category: {Category}";
    }
}