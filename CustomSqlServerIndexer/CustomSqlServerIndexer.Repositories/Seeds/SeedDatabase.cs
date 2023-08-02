namespace CustomSqlServerIndexer.Repositories;

public class SeedDatabase
{
    public static async Task SeedDataAsync(CustomSqlServerContext context)
    {
        await SeedTableHotel.SeedAsync(context);

    }
}