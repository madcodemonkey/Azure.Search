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
            CognitiveSearchEndpoint = config["CognitiveSearchEndpoint"],
            CognitiveSearchIndexName = config["CognitiveSearchIndexName"],
            CognitiveSearchKey = config["CognitiveSearchKey"],
            CognitiveSearchMaxUpsertBatchSize = config.GetValue<int>("CognitiveSearchMaxUpsertBatchSize"),
            CognitiveSearchSemanticConfigurationName = config["CognitiveSearchSemanticConfigurationName"],
        };

        var repositorySettings = new RepositorySettings()
        {
            ConnectionString = config["DatabaseConnectionString"],
            RunMigrationsOnStartup = config.GetValue<bool>("DatabaseRunMigrationsOnStartup")
        };

        sc.AddRepositories(repositorySettings);
        sc.AddServices(serviceSettings);

        return sc;
    }
}