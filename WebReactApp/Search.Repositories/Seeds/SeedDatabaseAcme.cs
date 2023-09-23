namespace Search.Repositories;

public class SeedDatabaseAcme
{
    public static async Task SeedDataAsync(AcmeContext context)
    {
        await SeedTableHotel.SeedAsync(context);
        await SeedIndexConfiguration.SeedAsync(context);
    }
}