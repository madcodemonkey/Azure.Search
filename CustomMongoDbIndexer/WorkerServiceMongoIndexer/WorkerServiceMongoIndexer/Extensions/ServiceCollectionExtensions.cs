using WorkerServiceMongoIndexer.Services;
using WorkerServiceMongoIndexer.Services.Extensions;

namespace WorkerServiceMongoIndexer;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomDependencies(this IServiceCollection sc, IConfiguration config)
    {
        var applicationSettings = new IndexerAppSettings();

        config.GetSection("AppSettings").Bind(applicationSettings);
        
        sc.AddServices(applicationSettings);

        return sc;
    }
}