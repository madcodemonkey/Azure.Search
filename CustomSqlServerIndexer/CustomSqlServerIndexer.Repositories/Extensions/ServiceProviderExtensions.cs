using CustomSqlServerIndexer.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CustomSqlServerIndexer.Repositories;

public static class ServiceProviderExtensions
{
    public static async Task ApplyDatabaseMigrationsAsync(this IServiceProvider serviceProvider)
    {
        var settings = serviceProvider.GetRequiredService<RepositorySettings>();

        // Should the migration be run?
        if (settings?.RunMigrationsOnStartup == false ||
            string.IsNullOrWhiteSpace(settings?.ConnectionString))
        {
            Console.WriteLine("Skipping migrations due to settings flag being false.");
            return;
        }

        Console.WriteLine("------START: Running migrations...");

        // Apply migrations at runtime: https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/applying?tabs=dotnet-core-cli#apply-migrations-at-runtime
        // This link also has a list of at least 5 reason why you should NOT run EF migrations in a production environment!
        using (var scope = serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<CustomSqlServerContext>();

            await context.Database.MigrateAsync();

            // Avoid seeding databases in upper environments.
            Console.WriteLine("------Seeding database...");
            await SeedDatabase.SeedDataAsync(context);
        }

        Console.WriteLine("------END: Migrations completed!");
    }
}