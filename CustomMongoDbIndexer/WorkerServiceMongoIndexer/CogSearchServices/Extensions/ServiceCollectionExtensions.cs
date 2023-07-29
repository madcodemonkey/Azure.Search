using Microsoft.Extensions.DependencyInjection;

namespace CogSearchServices.Services;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Used for Worker services that can't use scoped items!
    /// </summary>
    public static IServiceCollection AddCommonCogSearchServicesForWorkerBasedClients(this IServiceCollection sc, CogSearchServiceSettings settings)
    {
        AddCommon(sc, settings);

        sc.AddTransient<ICogSearchClientService, CogSearchClientService>();

        return sc;
    }

    /// <summary>
    /// Used for Web/Azure Function services that CAN use scoped items!
    /// </summary>
    public static IServiceCollection AddCogSearchServicesForWebBasedClients(this IServiceCollection sc, CogSearchServiceSettings settings)
    {
        AddCommon(sc, settings);

        sc.AddScoped<ICogSearchClientService, CogSearchClientService>();
        
        return sc;
    }

    private static IServiceCollection AddCommon(this IServiceCollection sc, CogSearchServiceSettings settings)
    {
        return sc.AddSingleton(settings);
    }
}