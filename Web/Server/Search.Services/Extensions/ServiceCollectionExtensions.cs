﻿using Microsoft.Extensions.DependencyInjection;
using Search.Repositories;

namespace Search.Services;

public static class ServiceCollectionExtensions
{
    public static void AddSearchServices(this IServiceCollection sc, SearchServiceSettings settings, AcmeDatabaseOptions databaseOptions)
    {
        sc.AddSingleton(settings);
        sc.AddSingleton(databaseOptions);
        
        sc.AddTransient<IHotelDataSourceService, HotelDataSourceService>();
        sc.AddTransient<IHotelIndexService, HotelIndexService>();
        sc.AddTransient<IHotelIndexerService, HotelIndexerService>();
        sc.AddTransient<IHotelFieldService, HotelFieldService>();
        sc.AddTransient<IHotelSearchService, HotelSearchService>();
        sc.AddTransient<IHotelSuggestorService, HotelSuggestorService>();
        sc.AddTransient<IHotelSynonymService, HotelSynonymService>();
    }
}