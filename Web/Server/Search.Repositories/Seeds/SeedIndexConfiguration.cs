using Search.Model;

namespace Search.Repositories;

public class SeedIndexConfiguration
{
    public static async Task SeedAsync(AcmeContext context)
    {
        if (context.IndexConfigurations.Any()) return;

        // Data for all environments!
        AddConfigurations(context);

        await context.SaveChangesAsync();
    }

    private static void AddConfigurations(AcmeContext context)
    {
        context.IndexConfigurations.Add(new IndexConfiguration()
        {
            IndexName = "hotels-idx",
            SecurityTrimmingField = nameof(Hotel.Roles).ToLower(),
            UsesCamelCaseFieldNames = true
        });
    } 
}