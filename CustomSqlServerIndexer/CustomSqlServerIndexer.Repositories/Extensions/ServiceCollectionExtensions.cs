using CogSimple.Services;
using CustomSqlServerIndexer.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CustomSqlServerIndexer.Repositories;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration config)
    {
        var settings = services.ConfigureSectionSettings<RepositorySettings>(config, RepositorySettings.SectionName);

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