using CogSimple.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OutOfTheBoxBlobIndexer.Services;

namespace OutOfTheBoxBlobIndexer;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomBlobIndexerDependencies(this IServiceCollection sc, IConfiguration config)
    {
        // Warning 1!! Use of  Environment.GetEnvironmentVariable("RunInformation") will not currently get you secrets.json overrides; thus, I'm injecting IConfiguration will does have the overrides!
        // Warning 2!! The use of config.GetSection("values").Bind(serviceSettings) does not appear to be using values from secrets.json!
        var serviceSettings = new ServiceSettings
        {
            CognitiveSearchDataSourceName = config["CognitiveSearchDataSourceName"],
            CognitiveSearchIndexerName = config["CognitiveSearchIndexerName"],
            CognitiveSearchIndexName = config["CognitiveSearchIndexName"],
            CognitiveSearchMaxUpsertBatchSize = config.GetValue<int>("CognitiveSearchMaxUpsertBatchSize"),
            CognitiveSearchSemanticConfigurationName = config["CognitiveSearchSemanticConfigurationName"],
            CognitiveSearchSoftDeleteFieldName = config["CognitiveSearchSoftDeleteFieldName"],
            CognitiveSearchSoftDeleteFieldValue = config["CognitiveSearchSoftDeleteFieldValue"],
            CognitiveSearchSuggestorName = config["CognitiveSearchSuggestorName"],
            StorageConnectionString = config["StorageConnectionString"],
            StorageContainerName = config["StorageContainerName"],
        };

        var cogClientSettings = new CogClientSettings()
        {
            CognitiveSearchEndpoint = config["CognitiveSearchEndpoint"],
            CognitiveSearchKey = config["CognitiveSearchKey"],
        };

        sc.AddServices(cogClientSettings, serviceSettings);

        return sc;
    }
}