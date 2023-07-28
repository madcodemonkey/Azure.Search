using IndexHelper.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SearchServices.Services;

namespace CustomBlobIndexer;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomBlobIndexerDependencies(this IServiceCollection sc, IConfiguration config)
    {
        // Warning 1!! Use of  Environment.GetEnvironmentVariable("RunInformation") will not currently get you secrets.json overrides; thus, I'm injecting IConfiguration will does have the overrides!
        // Warning 2!! The use of config.GetSection("values").Bind(serviceSettings) does not appear to be using values from secrets.json!
        var searchServiceSettings = new SearchServiceSettings
        {
            CognitiveSearchIndexName = config["CognitiveSearchIndexName"],
            CognitiveSearchKey = config["CognitiveSearchKey"],
            CognitiveSearchName = config["CognitiveSearchName"],
            CognitiveSearchSemanticConfigurationName = config["CognitiveSearchSemanticConfigurationName"],
        };
        sc.AddSearchServices(searchServiceSettings);

        
        var indexHelperSettings = new IndexHelperSettings();
        sc.AddServices(indexHelperSettings);

        return sc;
    }
}