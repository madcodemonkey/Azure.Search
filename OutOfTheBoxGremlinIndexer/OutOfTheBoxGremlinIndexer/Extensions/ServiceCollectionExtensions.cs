using CogSimple.Services;
using CustomSqlServerIndexer.Repositories;
using CustomSqlServerIndexer.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CustomSqlServerIndexer;

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
            CognitiveSearchSuggestorName = config["CognitiveSearchSuggestorName"]
        };

        var cogClientSettings = new CogClientSettings()
        {
            CognitiveSearchEndpoint = config["CognitiveSearchEndpoint"],
            CognitiveSearchKey = config["CognitiveSearchKey"],
        };

        var repositorySettings = new RepositorySettings
        {
            GremlinContainerName = config["GremlinContainerName"],
            GremlinDatabaseConnectionString = config["GremlinDatabaseConnectionString"],
            GremlinDatabaseName = config["GremlinDatabaseName"],
            GremlinEnableSSL = config.GetValue<bool>("GremlinEnableSSL"),
            GremlinHostName = config["GremlinHostName"],
            GremlinHostPort = config.GetValue<int>("GremlinHostPort"),
            GremlinKey = config["GremlinKey"],
            GremlinQueryType = config["GremlinQueryType"],
            GremlinSoftDeleteColumnName = config["GremlinSoftDeleteColumnName"],
            GremlinSoftDeleteColumnValue = config["GremlinSoftDeleteColumnValue"],
        };
        

        sc.AddRepositories(repositorySettings);
        sc.AddServices(serviceSettings, cogClientSettings);

        return sc;
    }
}