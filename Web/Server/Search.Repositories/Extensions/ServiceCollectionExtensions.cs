using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Search.Repositories;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAcmeRepositories(this IServiceCollection services, Action<AcmeDatabaseOptions> databaseOptions)
    {

        services.Configure(databaseOptions);
        services.TryAddEnumerable(ServiceDescriptor.Singleton<IValidateOptions<AcmeDatabaseOptions>, ValidateAcmeDatabaseOptions>());

        services.AddDbContext<AcmeContext>((serviceProvider, dbContextOptions) =>
        {
            var dbOptions = serviceProvider.GetRequiredService<IOptionsSnapshot<AcmeDatabaseOptions>>().Value;
            dbContextOptions.UseSqlServer(dbOptions.ConnectionString, sqlServerContextOptions =>
            {
                sqlServerContextOptions.EnableRetryOnFailure();
                sqlServerContextOptions.UseNetTopologySuite();
            });
        });

        services.AddScoped<IHotelRepository, HotelRepository>();
            
        return services;
    }
}