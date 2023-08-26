using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CogSimple.Services;

/// <summary>
/// Note: The Microsoft.Extensions.Options.ConfigurationExtensions Nuget package will get you the extensions needed for Bind and Configure off of IServiceCollection!
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCogSimpleServices(this IServiceCollection sc, IConfiguration config)
    {
        AssertIfSectionIsNotSetup(config);
        
        sc.Configure<CogClientSettings>(config.GetSection(CogClientSettings.SectionName));
        

        sc.AddScoped<ICogClientWrapperService, CogClientWrapperService>();
        sc.AddScoped<ICogSearchDataSourceService, CogSearchDataSourceService>();
        sc.AddScoped<ICogSearchIndexService, CogSearchIndexService>();
        sc.AddScoped<ICogSearchIndexerService, CogSearchIndexerService>();
        
        return sc;
    }

    private static void AssertIfSectionIsNotSetup(IConfiguration config)
    {
        var cogClientSettings = new CogClientSettings();
        config.GetSection(CogClientSettings.SectionName).Bind(cogClientSettings);

        if (string.IsNullOrWhiteSpace(cogClientSettings.Endpoint) ||
            cogClientSettings.Endpoint.StartsWith("---"))
        {
            throw new Exception($"Please add the {CogClientSettings.SectionName}:{nameof(cogClientSettings.Endpoint)} to your " +
                                $"local.settings.json files, secrets file or if deployed your configuration settings!");
        }

        if (string.IsNullOrWhiteSpace(cogClientSettings.Key) ||
            cogClientSettings.Endpoint.StartsWith("---"))
        {
            throw new Exception($"Please add the {CogClientSettings.SectionName}:{nameof(cogClientSettings.Key)} to your " +
                                $"local.settings.json files, secrets file or if deployed your configuration settings!");
        }

    }
}