using Microsoft.Extensions.DependencyInjection;

namespace Search.CogServices;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Use this method for web api, azure function and other web based projects.
    /// </summary>
    public static void AddCogServicesForWebBasedClients(this IServiceCollection sc, AcmeCogSettings settings)
    {
        AddCommon(sc, settings);
        
        sc.AddScoped<IAcmeCogAutoCompleteService, AcmeCogAutoCompleteService>();
        sc.AddScoped<IAcmeCogODataService, AcmeCogODataService>();
        sc.AddScoped<IAcmeCogOptionsService, AcmeCogOptionsService>();
        sc.AddScoped<IAcmeCogClientService, AcmeCogClientService>();
        sc.AddScoped<IAcmeCogDataSourceService, AcmeCogDataSourceService>();
        sc.AddScoped<IAcmeCogIndexerService, AcmeCogIndexerService>();
        sc.AddScoped<IAcmeCogIndexService, AcmeCogIndexService>();
        sc.AddScoped<IAcmeCogSearchService, AcmeCogSearchService>();
        sc.AddScoped<IAcmeCogSynonymService, AcmeCogSynonymService>();
        sc.AddScoped<IAcmeCogSuggestService, AcmeCogSuggestService>();
    }

    /// <summary>
    /// Use this method for worker Services that cannot handle scoped items!
    /// </summary>
    public static void AddCogServicesForWorkerServices(this IServiceCollection sc, AcmeCogSettings settings)
    {
        AddCommon(sc, settings);

        sc.AddTransient<IAcmeCogAutoCompleteService, AcmeCogAutoCompleteService>();
        sc.AddTransient<IAcmeCogOptionsService, AcmeCogOptionsService>();
        sc.AddTransient<IAcmeCogODataService, AcmeCogODataService>();
        sc.AddTransient<IAcmeCogClientService, AcmeCogClientService>();
        sc.AddTransient<IAcmeCogDataSourceService, AcmeCogDataSourceService>();
        sc.AddTransient<IAcmeCogIndexerService, AcmeCogIndexerService>();
        sc.AddTransient<IAcmeCogIndexService, AcmeCogIndexService>();
        sc.AddTransient<IAcmeCogSearchService, AcmeCogSearchService>();
        sc.AddTransient<IAcmeCogSynonymService, AcmeCogSynonymService>();
        sc.AddTransient<IAcmeCogSuggestService, AcmeCogSuggestService>();
    }

    private static IServiceCollection AddCommon(this IServiceCollection sc, AcmeCogSettings settings)
    {
        return sc.AddSingleton(settings);
    }
}