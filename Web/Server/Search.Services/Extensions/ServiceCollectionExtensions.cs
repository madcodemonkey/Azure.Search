using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Search.CogServices;
using Search.Repositories;

namespace Search.Services;

public static class ServiceCollectionExtensions
{
    public static void AddSearchServices(this IServiceCollection sc, SearchServiceSettings settings, AcmeDatabaseOptions databaseOptions)
    {
        sc.AddSingleton(settings);
        sc.AddSingleton(databaseOptions);

        sc.AddSingleton<IMemoryCache, MemoryCache>();

        sc.AddScoped<IAcmeOptionsService, AppOptionsService>(); // overriding an item in Search.CogServices!
        sc.AddScoped<IHotelDataSourceService, HotelDataSourceService>();
        sc.AddScoped<IHotelIndexerService, HotelIndexerService>();
        sc.AddScoped<IHotelIndexService, HotelIndexService>();
        sc.AddScoped<IHotelSynonymService, HotelSynonymService>();
        sc.AddScoped<IIndexConfigurationService, IndexConfigurationService>();
    }
}