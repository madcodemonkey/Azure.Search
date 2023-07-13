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
            BlobAccountName = config["BlobAccountName"],
            BlobContainerName = config["BlobContainerName"],
            BlobAccessKey = config["BlobAccessKey"],
        };

        sc.AddServices(serviceSettings);

        return sc;
    }
}