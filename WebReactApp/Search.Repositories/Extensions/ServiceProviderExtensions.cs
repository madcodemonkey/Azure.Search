using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Search.Repositories;

public static class ServiceProviderExtensions
{
    public static async Task ApplyDatabaseMigrationsAsync(this IServiceProvider serviceProvider)
    {
        var databaseOptions = serviceProvider.GetRequiredService<AcmeDatabaseOptions>();

        // Should the migration be run?
        if (databaseOptions?.RunMigrationsOnStartup == false ||
            string.IsNullOrWhiteSpace(databaseOptions?.ConnectionString))
        {
            return;
        }

        // Apply migrations at runtime: https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/applying?tabs=dotnet-core-cli#apply-migrations-at-runtime
        // This link also has a list of at least 5 reason why you should NOT run EF migrations in a production environment!
        using (var scope = serviceProvider.CreateScope())
        {
            var acmeContext = scope.ServiceProvider.GetRequiredService<AcmeContext>();

            await acmeContext.Database.MigrateAsync();

#if DEBUG
            // Avoid seeding databases in upper environments.
            await SeedDatabaseAcme.SeedDataAsync(acmeContext);
#endif
        }
    }
}