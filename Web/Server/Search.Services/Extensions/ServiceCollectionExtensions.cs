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

        sc.AddTransient<IAcmeOptionsService, AppOptionsService>(); // overriding an item in Search.CogServices!

        sc.AddTransient<IHotelDataSourceService, HotelDataSourceService>();
        sc.AddTransient<IHotelIndexerService, HotelIndexerService>();
        sc.AddTransient<IHotelIndexService, HotelIndexService>();
        sc.AddTransient<IHotelSynonymService, HotelSynonymService>();
    }
}