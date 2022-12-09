using System.ComponentModel.DataAnnotations;
using NetTopologySuite.Geometries;

namespace Search.Model;

public class Hotel
{
    public string? Amenities { get; set; }
    public double? BaseRate { get; set; }
    public string? Category { get; set; }
    public string? Description { get; set; }
    public string? DescriptionFr { get; set; }
    public string HotelName { get; set; } = null!;
    public int Id { get; set; }
    public bool IsDeleted { get; set; } = false;
    public DateTimeOffset? LastRenovationDate { get; set; }
    public Point? Location { get; set; }
    public bool? ParkingIncluded { get; set; }

    public int? Rating { get; set; }
    public string Roles { get; set; } = null!;

    [Timestamp]
    public byte[] RowVersion { get; set; }

    public bool? SmokingAllowed { get; set; }
}