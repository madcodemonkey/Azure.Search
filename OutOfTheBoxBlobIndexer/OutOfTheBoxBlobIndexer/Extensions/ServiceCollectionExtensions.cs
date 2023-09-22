using CogSimple.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OutOfTheBoxBlobIndexer.Models;
using OutOfTheBoxBlobIndexer.Services;

namespace OutOfTheBoxBlobIndexer;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomBlobIndexerDependencies(this IServiceCollection sc, IConfiguration config)
    {
        sc.ConfigureSectionSettings<OutOfTheBoxSettings>(config, OutOfTheBoxSettings.SectionName);

        sc.AddCogSimpleServices(config);
        sc.AddServices(config);

        return sc;
    }
}