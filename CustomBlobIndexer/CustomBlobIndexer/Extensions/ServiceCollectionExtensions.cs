using CogSimple.Services;
using CustomBlobIndexer.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CustomBlobIndexer;
 

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomBlobIndexerDependencies(this IServiceCollection sc, IConfiguration config)
    {
        sc.AddCogSimpleServices(config);
        
        sc.AddServices(config);

        return sc;
    }
}