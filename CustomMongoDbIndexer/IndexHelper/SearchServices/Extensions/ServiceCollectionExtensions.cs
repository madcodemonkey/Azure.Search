using Microsoft.Extensions.DependencyInjection;

namespace SearchServices.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSearchServices(this IServiceCollection sc, SearchServiceSettings settings)
    {
        sc.AddSingleton(settings);

        sc.AddScoped<ISearchClientService, SearchClientService>();
        
        return sc;
    }
}