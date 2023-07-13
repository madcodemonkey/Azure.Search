using CustomBlobIndexer.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CustomBlobIndexer;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomBlobIndexerDependencies(this IServiceCollection sc, IConfiguration config)
    {
        // Warning!! Use of  Environment.GetEnvironmentVariable("RunInformation") will not currently get you secrets.json overrides; thus, I'm injecting IConfiguration will does have the overrides!
        var serviceSettings = new ServiceSettings
        {
            BlobAccessKey = config["BlobAccessKey"],
            BlobAccountName = config["BlobAccountName"],
            BlobContainerName = config["BlobContainerName"],
            CognitiveSearchIndexName = config["CognitiveSearchIndexName"],
            CognitiveSearchKey = config["CognitiveSearchKey"],
            CognitiveSearchName = config["CognitiveSearchName"],
            CognitiveServiceEndpoint = config["CognitiveServiceEndpoint"],
            CognitiveServiceKey = config["CognitiveServiceKey"],
        };
        
        sc.AddServices(serviceSettings);

        return sc;
    }
}