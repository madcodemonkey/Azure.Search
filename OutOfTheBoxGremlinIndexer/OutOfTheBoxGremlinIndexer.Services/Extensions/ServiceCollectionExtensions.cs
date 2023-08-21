using CogSimple.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using OutOfTheBoxGremlinIndexer.Services;

namespace CustomSqlServerIndexer.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection sc, ServiceSettings settings, CogClientSettings cogClientSettings)
    {
        sc.AddCogSimpleServices(cogClientSettings);
        
        sc.AddSingleton(settings);
        sc.AddSingleton<IMemoryCache, MemoryCache>();
        
        sc.AddScoped<IGremlinDataService, GremlinDataService>();
        sc.AddScoped<IOutOfBoxService, OutOfBoxService>();

        return sc;
    }
}