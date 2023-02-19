using Microsoft.Extensions.DependencyInjection;

namespace Search.CogServices;

public static class ServiceCollectionExtensions
{
    public static void AddCogServices(this IServiceCollection sc, AcmeSearchSettings settings)
    {
        sc.AddSingleton(settings);

        sc.AddSingleton<IAcmeIndexInfoService, AcmeIndexInfoService>();

        sc.AddScoped<IAcmeAutoCompleteService, AcmeAutoCompleteService>();
        sc.AddScoped<IAcmeODataService, AcmeODataService>();
        sc.AddScoped<IAcmeSearchDataSourceService, AcmeSearchDataSourceService>();
        sc.AddScoped<IAcmeSearchIndexerService, AcmeSearchIndexerService>();
        sc.AddScoped<IAcmeSearchIndexService, AcmeSearchIndexService>();
        sc.AddScoped<IAcmeSearchService, AcmeSearchService>();
        sc.AddScoped<IAcmeSearchSynonymService, AcmeSearchSynonymService>();
        sc.AddScoped<IAcmeSuggestorService, AcmeSuggestorService>();
    }
}