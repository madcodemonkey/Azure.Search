using Microsoft.Extensions.DependencyInjection;
using OutOfTheBoxBlobIndexer.Services.Services;

namespace OutOfTheBoxBlobIndexer.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection sc, ServiceSettings settings)
    {
        sc.AddSingleton(settings); 

        sc.AddScoped<ICogClientWrapperService, CogClientWrapperService>();
        sc.AddScoped<ICogSearchDataSourceService, CogSearchDataSourceService>();
        sc.AddScoped<ICustomSearchIndexService, CustomSearchIndexService>();
        sc.AddScoped<IOutOfBoxService, OutOfBoxService>();

        return sc;
    }
}