using CustomSqlServerIndexer.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CustomSqlServerIndexer.Repositories;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services, RepositorySettings settings)
    {
        services.AddSingleton(settings);
     
        services.AddDbContext<CustomSqlServerContext>((serviceProvider, dbContextOptions) =>
        {
            dbContextOptions.UseSqlServer(settings.ConnectionString, sqlServerContextOptions =>
            {
                sqlServerContextOptions.EnableRetryOnFailure();
            });
        });

        services.AddScoped<IHotelRepository, HotelRepository>();
            
        return services;
    }
}