using CogSimple.Services;
using VectorExample.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace VectorExample;
 

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomBlobIndexerDependencies(this IServiceCollection sc, IConfiguration config)
    {
        sc.AddCogSimpleServices(config);
        
        sc.AddServices(config);

        return sc;
    }
}