using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace CustomSqlServerIndexer.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection sc, ServiceSettings settings)
    {
        sc.AddSingleton(settings);
        sc.AddSingleton<IMemoryCache, MemoryCache>();

        sc.AddScoped<ICogClientWrapperService, CogClientWrapperService>();
        sc.AddScoped<ICogSearchDataSourceService, CogSearchDataSourceService>();
        sc.AddScoped<ICustomSearchIndexService, CustomSearchIndexService>();
        sc.AddScoped<IGremlinCogSearchService, GremlinCogSearchService>();
        sc.AddScoped<IGremlinDataService, GremlinDataService>();
        
        return sc;
    }
}