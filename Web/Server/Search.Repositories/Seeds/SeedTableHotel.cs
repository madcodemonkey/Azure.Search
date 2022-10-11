using Search.Model;
using Search.Repositories;

namespace Paizo.Starfinder.Repositories
{
    public class SeedTableHotel
    {
        public static async Task SeedAsync(AcmeContext context)
        {
            if (context.Hotels.Any()) return;

            // Data for all environments!
            AddHotels(context);

            await context.SaveChangesAsync();
        }

        private static void AddHotels(AcmeContext context)
        {
            var hotel1 = new Hotel
            {
                BaseRate = 199,
                Category = "Luxury",
                Description = "Best hotel in town",
                DescriptionFr = "Meilleur hôtel en ville",
                HotelName = "Fancy Stay",
                Amenities = "[\"pool\", \"view\", \"wifi\", \"concierge\"]",
                IsDeleted = false,
                LastRenovationDate = new DateTimeOffset(2010, 6, 27, 0, 0, 0, 0, TimeSpan.FromSeconds(0)),
                ParkingIncluded = false,
                Rating = 5,
                Roles = "[\"admin\", \"member\"]",
                SmokingAllowed = false,
                Location = new NetTopologySuite.Geometries.Point(-122.131577d, 47.678581d) { SRID = 4326 }
            };

            context.Hotels.Add(hotel1);

            var hotel2 = new Hotel
            {
                BaseRate = 79.99,
                Category = "Budget",
                Description = "Cheapest hotel in town",
                DescriptionFr = "Hôtel le moins cher en ville",
                HotelName = "Roach Motel",
                Amenities = "[\"motel\", \"budget\"]",
                IsDeleted = false,
                LastRenovationDate = new DateTimeOffset(1982, 4, 28, 0, 0, 0, 0, TimeSpan.FromSeconds(0)),
                ParkingIncluded = true,
                Rating = 1,
                Roles = "[\"nonmember\", \"admin\", \"member\"]",
                SmokingAllowed = true,
                Location = new NetTopologySuite.Geometries.Point(-122.131577d, 49.678581d) { SRID = 4326 }
            };

            context.Hotels.Add(hotel2);

            var hotel3 = new Hotel
            {
                BaseRate = 129.99,
                Category = null,
                Description = "Close to town hall and the river",
                DescriptionFr = null,
                HotelName = "Downtown Hotel",
                Amenities = null,
                IsDeleted = false,
                LastRenovationDate = null,
                ParkingIncluded = null,
                Rating = null,
                Roles = "[\"nonmember\", \"admin\", \"member\"]",
                SmokingAllowed = null,
                Location = null
            };

            context.Hotels.Add(hotel3);

            var hotel4 = new Hotel
            {
                BaseRate = 247.19,
                Category = null,
                Description = "Super secret hideout",
                DescriptionFr = null,
                HotelName = "Secret Motel",
                Amenities = null,
                IsDeleted = false,
                LastRenovationDate = null,
                ParkingIncluded = null,
                Rating = null,
                Roles = "[\"admin\"]",
                SmokingAllowed = null,
                Location = null
            };

            context.Hotels.Add(hotel4);
        }
    }
}