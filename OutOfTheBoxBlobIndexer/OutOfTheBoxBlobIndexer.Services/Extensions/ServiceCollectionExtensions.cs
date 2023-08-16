using CogSimple.Services;
using Microsoft.Extensions.DependencyInjection;

namespace OutOfTheBoxBlobIndexer.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection sc, CogClientSettings cogClientSettings, ServiceSettings settings)
    {
        sc.AddCogSimpleServices(cogClientSettings);
        sc.AddSingleton(settings);

        sc.AddScoped<IOutOfBoxService, OutOfBoxService>();

        return sc;
    }
}