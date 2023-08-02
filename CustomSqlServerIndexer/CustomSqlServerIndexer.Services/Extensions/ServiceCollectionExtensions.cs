using Microsoft.Extensions.DependencyInjection;

namespace CustomSqlServerIndexer.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection sc, ServiceSettings settings)
    {
        sc.AddSingleton(settings);

        sc.AddTransient<ICustomSearchIndexService, CustomSearchIndexService>();

        return sc;
    }
}