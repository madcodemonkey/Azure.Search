using System.Text;
using System.Text.Json;
using CustomSqlServerIndexer.Models;

namespace CustomSqlServerIndexer.Repositories;

public class SeedTableHotel
{
    public static async Task SeedAsync(CustomSqlServerContext context)
    {
        if (context.Hotels.Any()) return;

        // Data for all environments!
        AddHotels(context);

        await context.SaveChangesAsync();
    }

    private static void AddHotels(CustomSqlServerContext context)
    {
        Random rand = new Random(DateTime.Now.Millisecond);

        context.Hotels.Add(CreateOne(rand, "Fancy Stay", "Best hotel in town", "Meilleur hôtel en ville", longitude: -122.131577d, latitude: 47.678581d, baseRate: 450.99));
        context.Hotels.Add(CreateOne(rand, "Roach Motel", "Cheapest hotel in town", "Hôtel le moins cher en ville", longitude: -122.131577d, latitude: 49.678581d, baseRate: 79.99));
        context.Hotels.Add(CreateOne(rand, "Downtown Motel", "Close to town hall and the river"));

        context.Hotels.Add(CreateOne(rand, "Homewood Suites by Hilton", "Near the harbor"));
        context.Hotels.Add(CreateOne(rand, "Fairmont Olympic Hotel", "It's grey!"));
        context.Hotels.Add(CreateOne(rand, "Executive Hotel Pacific", "Near a stoplight"));

        context.Hotels.Add(CreateOne(rand, "Hyatt House Seattle/Downtown", "It's downtown"));
        context.Hotels.Add(CreateOne(rand, "Warwick Seattle", "We have balconies"));
        context.Hotels.Add(CreateOne(rand, "Hilton Garden Inn Seattle Downtown", "Room with a view, but no murders!"));
        context.Hotels.Add(CreateOne(rand, "Motif Seattle - Destination by Hyatt", "Skyviews"));
        context.Hotels.Add(CreateOne(rand, "Moore Hotel", "Power lines are visible!"));
        context.Hotels.Add(CreateOne(rand, "Sheraton Grand Seattle", "Triangles on the carpet"));
        context.Hotels.Add(CreateOne(rand, "Mediterranean Inn", "It's yellow and white"));
        context.Hotels.Add(CreateOne(rand, "Palihotel Seattle", "Looks like it's on main street"));
        context.Hotels.Add(CreateOne(rand, "The Westin Seattle", "Big headboards"));
        context.Hotels.Add(CreateOne(rand, "Green Tortoise Hostel Seattle", "You can see the space needle"));
        context.Hotels.Add(CreateOne(rand, "The Inn at Virginia Mason", "It has a rooftop patio"));
        context.Hotels.Add(CreateOne(rand, "Lotte Hotel Seattle", "Grey front that looks like a bank"));
        context.Hotels.Add(CreateOne(rand, "Travelodge by Wyndham Seattle By The Space Needle", "That 60s vibe"));
        context.Hotels.Add(CreateOne(rand, "ilver Cloud Hotel - Seattle Stadium", "Yup, it's a hotel"));

        context.Hotels.Add(CreateOne(rand, "Embassy Suites by Hilton Washington DC Convention Center", "We have a flag!"));
        context.Hotels.Add(CreateOne(rand, "Beacon Hotel & Corporate Quarters", "Cobblestone driveway.  Has a Vegas vibe!"));
        context.Hotels.Add(CreateOne(rand, "YOTEL Washington DC", "We think we are cleaver with your hotel name"));
        context.Hotels.Add(CreateOne(rand, "Days Inn by Wyndham Washington DC/Connecticut Avenue", "Plain and boring"));
        context.Hotels.Add(CreateOne(rand, "Hyatt Place Washington Dc/Us Capitol", "It's another hotel!"));
        
        // Secret hotels
        context.Hotels.Add(CreateOne(rand, "Super Motel", "Secret super hideout", isSecret: true));
        context.Hotels.Add(CreateOne(rand, "The Ven at Embassy Row", "Secret view", isSecret: true));
        context.Hotels.Add(CreateOne(rand, "Moxy Washington", "Secret spies stay here.  Shhh!!!", isSecret: true));
        context.Hotels.Add(CreateOne(rand, "Omni Shoreham Hotel", "Secret They'll never know you stayed here!", isSecret: true));
    }

    private static Hotel CreateOne(Random rand, 
        string hotelName, string? description, string? descriptionInFrench = null,
        bool isSecret = false, 
        double? longitude = null, double? latitude = null, double? baseRate = null)
    {
        double cost =  baseRate ?? rand.Next(80, 400) * 1.0d + rand.Next(0, 99) * .01d;
      
        return  new Hotel
        {
            BaseRate = cost,
            HotelName = hotelName,
            Category = AddCategories(cost),
            Description = description,
            DescriptionFr = descriptionInFrench,
            Amenities = AddAmenities(rand, cost),
            IsDeleted = false,
            LastRenovationDate = AddLastRenovationDate(rand),
            ParkingIncluded = rand.Next(1, 100) > 10,
            Rating = rand.Next(1, 5),
            Roles = AddRoles(rand, isSecret),
            SmokingAllowed = cost < 100, // Cheap hotels let you smoke
        };
    }

    private static DateTimeOffset AddLastRenovationDate(Random rand)
    {
        var daysSinceLastRenovation = rand.Next(30, (365 * 8)) * -1;
        DateTime renovationDate = DateTime.Now.AddDays(daysSinceLastRenovation);
        return new DateTimeOffset(renovationDate.Year, renovationDate.Month, renovationDate.Day, 0, 0, 0, 0, TimeSpan.FromSeconds(0));
    }

    private static string? AddAmenities(Random rand, double cost)
    {
        int aboveWhat = cost > 300 ? 10 : 50;  // The higher the cost the better the odds of having the amenity
        var possibleAmenities = new List<string> { "pool", "view","wifi","concierge" };
        var actualAmenities = new List<string>();
        foreach (var possibleAmenity in possibleAmenities)
        {
            if (rand.Next(1, 100) > aboveWhat)
                actualAmenities.Add(possibleAmenity);
        }
        
        return actualAmenities.Count > 0 ? JsonSerializer.Serialize(actualAmenities) : null;
    }


    private static string AddRoles(Random rand, bool isSecret)
    {
        var sb = new StringBuilder();
        sb.Append("admin");

        var possibleNonAdminRoles = new List<string> { "nonmember", "member" };

        if (isSecret == false)
        {
            foreach (var possibleRole in possibleNonAdminRoles)
            {
                if (rand.Next(1, 100) > 50)
                {
                    sb.Append(",");
                    sb.Append(possibleRole);
                }
            }
        }

        return sb.ToString();
    }

    

    private static string? AddCategories(double cost)
    {
        return cost switch
        {
            < 100 => "Budget",
            >= 300 => "Luxury",
            _ => null
        };
    }
}