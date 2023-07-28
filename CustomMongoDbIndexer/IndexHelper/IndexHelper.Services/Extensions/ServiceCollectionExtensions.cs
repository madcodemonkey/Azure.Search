using IndexHelper.Models;
using Microsoft.Extensions.DependencyInjection;

namespace IndexHelper.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection sc, IndexAppSettings settings)
    {
        sc.AddRepositories();

        sc.AddSingleton(settings);

        sc.AddTransient<IPersonIndexService, PersonIndexService>();
        sc.AddTransient<IPersonMongoService, PersonMongoService>();
        


        return sc;
    }
}