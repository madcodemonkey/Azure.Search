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
            CognitiveSearchEndpoint = config["CognitiveSearchEndpoint"],
            CognitiveSearchIndexerName = config["CognitiveSearchIndexerName"],
            CognitiveSearchIndexName = config["CognitiveSearchIndexName"],
            CognitiveSearchKey = config["CognitiveSearchKey"],
            CognitiveSearchMaxUpsertBatchSize = config.GetValue<int>("CognitiveSearchMaxUpsertBatchSize"),
            CognitiveSearchSemanticConfigurationName = config["CognitiveSearchSemanticConfigurationName"],
        };

        var repositorySettings = new RepositorySettings
        {
            GremlinContainerName = config["GremlinContainerName"],
            GremlinDatabaseConnectionString = config["GremlinDatabaseConnectionString"],
            GremlinDatabaseName = config["GremlinDatabaseName"],
            GremlinHostName = config["GremlinHostName"],
            GremlinKey = config["GremlinKey"],
        };

        sc.AddRepositories(repositorySettings);
        sc.AddServices(serviceSettings);

        return sc;
    }
}