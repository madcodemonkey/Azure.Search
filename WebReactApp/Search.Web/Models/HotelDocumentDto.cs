using Microsoft.Spatial;

namespace Search.Web.Models;

public class HotelDocumentDto
{
    public double? BaseRate { get; set; }
    public string Category { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string DescriptionFr { get; set; } = string.Empty;
    public string HotelId { get; set; } = string.Empty;
    public string HotelName { get; set; } = string.Empty;
    public DateTimeOffset? LastRenovationDate { get; set; }
    public GeographyPoint Location { get; set; }
    public bool? ParkingIncluded { get; set; }
    public int? Rating { get; set; }
    public bool? SmokingAllowed { get; set; }
    public string[] Tags { get; set; } = Array.Empty<string>();
}