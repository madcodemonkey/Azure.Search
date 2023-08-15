﻿using Microsoft.Extensions.DependencyInjection;
using OutOfTheBoxBlobIndexer.Services.Services;

namespace OutOfTheBoxBlobIndexer.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection sc, CogClientSettings cogClientSettings, ServiceSettings settings)
    {
        sc.AddSingleton(cogClientSettings);
        sc.AddSingleton(settings);

        sc.AddScoped<ICogClientWrapperService, CogClientWrapperService>();
        sc.AddScoped<ICogSearchDataSourceService, CogSearchDataSourceService>();
        sc.AddScoped<ICogSearchIndexService, CogSearchIndexService>();
        sc.AddScoped<IOutOfBoxService, OutOfBoxService>();

        return sc;
    }
}