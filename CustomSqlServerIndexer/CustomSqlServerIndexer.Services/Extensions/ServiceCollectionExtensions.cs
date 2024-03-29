﻿using CogSimple.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CustomSqlServerIndexer.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection sc, IConfiguration config)
    {
        sc.AddSingleton<IMemoryCache, MemoryCache>();
        sc.AddScoped<ICustomSearchIndexService, CustomSearchIndexService>();
        sc.AddScoped<ICustomSqlServerIndexerService, CustomSqlServerIndexerService>();
        sc.AddScoped<IHighWaterMarkStorageService, HighWaterMarkStorageService>();
        
        return sc;
    }
}