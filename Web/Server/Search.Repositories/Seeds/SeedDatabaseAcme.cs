using Search.Repositories;

namespace Paizo.Starfinder.Repositories
{
    public class SeedDatabaseAcme
    {
        public static async Task SeedDataAsync(AcmeContext context)
        {
            await SeedTableHotel.SeedAsync(context);

        }
    }
}
