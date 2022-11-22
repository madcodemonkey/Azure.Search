using Microsoft.Extensions.DependencyInjection;

namespace Search.CogServices;

public static class ServiceCollectionExtensions
{
    public static void AddCogServices(this IServiceCollection sc, AcmeSearchSettings settings)
    {
        sc.AddSingleton(settings);

        sc.AddScoped<IAcmeSearchDataSourceService, AcmeSearchDataSourceService>();
        sc.AddScoped<IAcmeSearchIndexService, AcmeSearchIndexService>();
        sc.AddScoped<IAcmeSearchIndexerService, AcmeSearchIndexerService>();
        sc.AddTransient<IAcmeSearchSynonymService, AcmeSearchSynonymService>();
    }
}