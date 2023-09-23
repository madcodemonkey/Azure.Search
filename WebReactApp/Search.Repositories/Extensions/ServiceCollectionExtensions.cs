using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Search.Repositories;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAcmeRepositories(this IServiceCollection services, AcmeDatabaseOptions databaseOptions)
    {

        services.AddSingleton(databaseOptions);
        services.TryAddEnumerable(ServiceDescriptor.Singleton<IValidateOptions<AcmeDatabaseOptions>, ValidateAcmeDatabaseOptions>());

        services.AddDbContext<AcmeContext>((serviceProvider, dbContextOptions) =>
        {
            dbContextOptions.UseSqlServer(databaseOptions.ConnectionString, sqlServerContextOptions =>
            {
                sqlServerContextOptions.EnableRetryOnFailure();
                sqlServerContextOptions.UseNetTopologySuite();
            });
        });

        services.AddScoped<IHotelRepository, HotelRepository>();
        services.AddScoped<IIndexConfigurationRepository, IndexConfigurationRepository>();
        
        return services;
    }
}