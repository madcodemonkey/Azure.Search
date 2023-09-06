using CogSimple.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace OutOfTheBoxBlobIndexer.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection sc, IConfiguration config)
    {
        sc.AddCogSimpleServices(config);
  
        sc.AddScoped<IOutOfBoxService, OutOfBoxService>();

        return sc;
    }
}