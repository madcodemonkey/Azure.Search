using NetTopologySuite.Geometries;

namespace Search.Model;

public class Hotel
{
    public int Id { get; set; }

    public double? BaseRate { get; set; }
    public string? Category { get; set; }

    public string? Description { get; set; }

    public string? DescriptionFr { get; set; }

    public string HotelName { get; set; } = null!;

    public string? Amenities { get; set; }

    public bool? ParkingIncluded { get; set; }

    public bool? SmokingAllowed { get; set; }

    public DateTimeOffset? LastRenovationDate { get; set; }

    public int? Rating { get; set; }

    public string Roles { get; set; } = null!;

    public Point? Location { get; set; }
    public bool IsDeleted { get; set; } = false;
}

