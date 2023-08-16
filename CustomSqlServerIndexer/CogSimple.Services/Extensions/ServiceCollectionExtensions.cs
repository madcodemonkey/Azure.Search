using Microsoft.Extensions.DependencyInjection;

namespace CogSimple.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCogSimpleServices(this IServiceCollection sc, CogClientSettings cogClientSettings)
    {
        sc.AddSingleton(cogClientSettings);
        
        sc.AddScoped<ICogClientWrapperService, CogClientWrapperService>();
        sc.AddScoped<ICogSearchDataSourceService, CogSearchDataSourceService>();
        sc.AddScoped<ICogSearchIndexService, CogSearchIndexService>();
        sc.AddScoped<ICogSearchIndexerService, CogSearchIndexerService>();
        
        return sc;
    }
}