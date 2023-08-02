using IndexHelper.Models;
using IndexHelper.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDBServices;
using Search.CogServices;

namespace CustomBlobIndexer;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomBlobIndexerDependencies(this IServiceCollection sc, IConfiguration config)
    {
        // Warning 1!! Use of  Environment.GetEnvironmentVariable("RunInformation") will not currently get you secrets.json overrides; thus, I'm injecting IConfiguration will does have the overrides!
        // Warning 2!! The use of config.GetSection("values").Bind(serviceSettings) does not appear to be using values from secrets.json!
        var cogSearchSettings = new AcmeCogSettings
        {
            CognitiveSearchKey = config["CognitiveSearchKey"],
            CognitiveSearchEndPoint = config["CognitiveSearchEndPoint"]
        };
        sc.AddCogServicesForWebBasedClients(cogSearchSettings);


        var mongoSettings = new MongoDBServiceSettings
        {
            MongoAtlasConnectionString = config["MongoAtlasConnectionString"],
        };
        sc.AddCommonMongoDBServices(mongoSettings);


        var indexAppSettings = new IndexAppSettings
        {
            CognitiveSearchIndexName = config["CognitiveSearchIndexName"],
            CognitiveSearchSemanticConfigurationName = config["CognitiveSearchSemanticConfigurationName"],
            MongoDatabaseName = config["MongoDatabaseName"] ,
            MongoPeopleCollection = config["MongoPeopleCollection"]
        };

        sc.AddServices(indexAppSettings);

        return sc;
    }
}