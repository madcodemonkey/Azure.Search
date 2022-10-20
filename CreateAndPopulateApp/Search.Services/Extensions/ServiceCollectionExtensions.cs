using Microsoft.Extensions.DependencyInjection;

namespace Search.Services;

public static class ServiceCollectionExtensions
{
    public static void AddSearchServices(this IServiceCollection sc, SearchServiceSettings settings)
    {
        sc.AddSingleton(settings);
            
            
        sc.AddTransient<IHotelIndexerService, HotelIndexerService>();
        sc.AddTransient<IHotelSynonymService, HotelSynonymService>();
            
        sc.AddScoped<IAcmeSearchDataSourceService, AcmeSearchDataSourceService>();
        sc.AddScoped<IAcmeSearchIndexService, AcmeSearchIndexService>();
        sc.AddScoped<IAcmeSearchIndexerService, AcmeSearchIndexerService>();
        sc.AddTransient<IAcmeSearchSynonymService, AcmeSearchSynonymService>();
    }
}