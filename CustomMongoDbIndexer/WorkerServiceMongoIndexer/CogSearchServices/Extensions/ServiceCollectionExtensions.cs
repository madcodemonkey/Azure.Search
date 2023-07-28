using Microsoft.Extensions.DependencyInjection;

namespace CogSearchServices.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCommonCogSearchServices(this IServiceCollection sc, CogSearchServiceSettings settings)
    {
        sc.AddSingleton(settings);

        sc.AddScoped<ICogSearchClientService, CogSearchClientService>();
        
        return sc;
    }
}