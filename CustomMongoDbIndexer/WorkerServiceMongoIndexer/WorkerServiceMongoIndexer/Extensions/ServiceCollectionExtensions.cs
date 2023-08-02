using MongoDBServices;
using Search.CogServices;
using WorkerServiceMongoIndexer.Services;
using WorkerServiceMongoIndexer.Services.Extensions;

namespace WorkerServiceMongoIndexer;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomDependencies(this IServiceCollection sc, IConfiguration config)
    {
        var cogSearchSettings = new AcmeCogSettings();
        config.GetSection("SearchSettings").Bind(cogSearchSettings);
        sc.AddCogServicesForWorkerServices(cogSearchSettings);

        var mongoSettings = new MongoDBServiceSettings();
        config.GetSection("MongoSettings").Bind(mongoSettings);
        sc.AddCommonMongoDBServices(mongoSettings);

        var indexerAppSettings = new IndexerAppSettings();
        config.GetSection("IndexerSettings").Bind(indexerAppSettings);
        sc.AddIndexerServices(indexerAppSettings);

        return sc;
    }
}