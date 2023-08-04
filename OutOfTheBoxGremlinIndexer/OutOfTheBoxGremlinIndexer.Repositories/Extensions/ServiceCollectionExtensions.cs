using CustomSqlServerIndexer.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CustomSqlServerIndexer.Repositories;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services, RepositorySettings settings)
    {
        services.AddSingleton(settings);
     
     
       //  services.AddScoped<IHotelRepository, HotelRepository>();
            
        return services;
    }
}