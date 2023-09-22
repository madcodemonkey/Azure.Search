using CogSimple.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OutOfTheBoxGremlinIndexer.Models;
using OutOfTheBoxGremlinIndexer.Repositories;
using OutOfTheBoxGremlinIndexer.Services;

namespace CustomSqlServerIndexer;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomBlobIndexerDependencies(this IServiceCollection sc, IConfiguration config)
    {
        sc.ConfigureSectionSettings<OutOfTheBoxSettings>(config, OutOfTheBoxSettings.SectionName);

        sc.AddCogSimpleServices(config);
        sc.AddRepositories(config);
        sc.AddServices(config);

        return sc;
    }
}