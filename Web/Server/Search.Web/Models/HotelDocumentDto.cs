using Microsoft.Spatial;

namespace Search.Web.Models;

public class HotelDocumentDto
{
    public string HotelId { get; set; }

    public double? BaseRate { get; set; }

    public string Description { get; set; }

    public string DescriptionFr { get; set; }

    public string HotelName { get; set; }

    public string Category { get; set; }

    public string[] Tags { get; set; }

    public bool? ParkingIncluded { get; set; }

    public bool? SmokingAllowed { get; set; }

    public DateTimeOffset? LastRenovationDate { get; set; }

    public int? Rating { get; set; }

    public GeographyPoint Location { get; set; }
}