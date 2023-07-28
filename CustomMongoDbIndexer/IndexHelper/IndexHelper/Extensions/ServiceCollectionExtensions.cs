using CogSearchServices.Services;
using IndexHelper.Models;
using IndexHelper.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDBServices;

namespace CustomBlobIndexer;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomBlobIndexerDependencies(this IServiceCollection sc, IConfiguration config)
    {
        // Warning 1!! Use of  Environment.GetEnvironmentVariable("RunInformation") will not currently get you secrets.json overrides; thus, I'm injecting IConfiguration will does have the overrides!
        // Warning 2!! The use of config.GetSection("values").Bind(serviceSettings) does not appear to be using values from secrets.json!
        var cogSearchSettings = new CogSearchServiceSettings
        {
            CognitiveSearchIndexName = config["CognitiveSearchIndexName"],
            CognitiveSearchKey = config["CognitiveSearchKey"],
            CognitiveSearchName = config["CognitiveSearchName"],
        };
        sc.AddCommonCogSearchServices(cogSearchSettings);


        var mongoSettings = new MongoDBServiceSettings
        {
            MongoAtlasConnectionString = config["MongoAtlasConnectionString"],
        };
        sc.AddCommonMongoDBServices(mongoSettings);


        var indexAppSettings = new IndexAppSettings
        {
            CognitiveSearchSemanticConfigurationName = config["CognitiveSearchSemanticConfigurationName"],
            MongoDatabaseName = config["MongoDatabaseName"] ,
            MongoPeopleCollection = config["MongoPeopleCollection"]
        };

        sc.AddServices(indexAppSettings);

        return sc;
    }
}