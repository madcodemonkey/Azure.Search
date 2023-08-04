using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace CustomSqlServerIndexer.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection sc, ServiceSettings settings)
    {
        sc.AddSingleton(settings);
        sc.AddSingleton<IMemoryCache, MemoryCache>();
        sc.AddScoped<ICustomSearchIndexService, CustomSearchIndexService>();
        
        return sc;
    }
}