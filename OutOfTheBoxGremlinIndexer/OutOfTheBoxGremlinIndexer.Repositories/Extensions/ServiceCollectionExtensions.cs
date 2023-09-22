using CogSimple.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace OutOfTheBoxGremlinIndexer.Repositories;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration config)
    {
        services.ConfigureSectionSettings<RepositorySettings>(config, RepositorySettings.SectionName);

        services.AddScoped<IGremlinClientWrapper, GremlinClientWrapper>();
        services.AddScoped<IGremlinDataRepository, GremlinDataRepository>();
        

        return services;
    }
}