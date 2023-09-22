using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace OutOfTheBoxGremlinIndexer.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection sc, IConfiguration config)
    {
        sc.AddSingleton<IMemoryCache, MemoryCache>();
      
        sc.AddScoped<IGremlinDataService, GremlinDataService>();
        sc.AddScoped<IOutOfBoxService, OutOfBoxService>();

        return sc;
    }
}