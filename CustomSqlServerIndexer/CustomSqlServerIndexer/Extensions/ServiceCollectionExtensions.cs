using CogSimple.Services;
using CustomSqlServerIndexer.Models;
using CustomSqlServerIndexer.Repositories;
using CustomSqlServerIndexer.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CustomSqlServerIndexer;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomBlobIndexerDependencies(this IServiceCollection sc, IConfiguration config)
    {
        sc.ConfigureSectionSettings<CustomIndexerSettings>(config, CustomIndexerSettings.SectionName);

        sc.AddCogSimpleServices(config);
        sc.AddRepositories(config);
        sc.AddServices(config);

        return sc;
    }
}