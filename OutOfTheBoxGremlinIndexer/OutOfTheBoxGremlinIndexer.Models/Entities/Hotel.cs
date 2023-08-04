using System.ComponentModel.DataAnnotations;

namespace CustomSqlServerIndexer.Models;

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
    public bool? ParkingIncluded { get; set; }

    public int? Rating { get; set; }
    public string Roles { get; set; } = null!;

    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();

    public bool? SmokingAllowed { get; set; }
}