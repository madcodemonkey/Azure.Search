using Microsoft.Extensions.DependencyInjection;

namespace WorkerServiceMongoIndexer.Services.Extensions;


public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection sc, IndexerAppSettings settings)
    {
        sc.AddSingleton(settings);
        sc.AddSingleton<IMongoClientService, MongoClientService>();
        sc.AddSingleton<IMongoResumeTokenService, MongoResumeTokenService>();

        
        sc.AddTransient<IMonitorDatabaseService, MonitorDatabaseService>();
        sc.AddTransient<IMongoDocumentProcessorService, MongoDocumentProcessorService>();

        return sc;
    }
}