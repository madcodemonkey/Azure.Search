using Microsoft.Extensions.DependencyInjection;

namespace Search.Services;

public static class ServiceCollectionExtensions
{
    public static void AddSearchServices(this IServiceCollection sc, SearchServiceSettings settings)
    {
        sc.AddSingleton(settings);

        sc.AddTransient<IHotelIndexerService, HotelIndexerService>();
        sc.AddTransient<IHotelSynonymService, HotelSynonymService>();

        sc.AddScoped<ISearchDataSourceService, SearchDataSourceService>();
        sc.AddScoped<ISearchIndexService, SearchIndexService>();
        sc.AddScoped<ISearchIndexerService, SearchIndexerService>();
        sc.AddTransient<ISearchSynonymService, SearchSynonymService>();
    }
}