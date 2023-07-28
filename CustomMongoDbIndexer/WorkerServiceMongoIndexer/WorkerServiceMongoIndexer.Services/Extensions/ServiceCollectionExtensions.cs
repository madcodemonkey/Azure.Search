using Microsoft.Extensions.DependencyInjection;

namespace WorkerServiceMongoIndexer.Services.Extensions;


public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddIndexerServices(this IServiceCollection sc, IndexerAppSettings settings)
    {
        sc.AddSingleton(settings);
        sc.AddSingleton<IMongoResumeTokenService, MongoResumeTokenService>();

        
        sc.AddTransient<IMonitorDatabaseService, MonitorDatabaseService>();
        sc.AddTransient<IMongoDocumentProcessorService, MongoDocumentProcessorService>();
        sc.AddTransient<IPersonIndexService, PersonIndexService>();

        return sc;
    }
}