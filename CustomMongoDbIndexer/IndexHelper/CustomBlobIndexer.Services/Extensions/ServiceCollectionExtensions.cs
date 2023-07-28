using Microsoft.Extensions.DependencyInjection;

namespace IndexHelper.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection sc, IndexHelperSettings settings)
    {
        sc.AddSingleton(settings);
        
        sc.AddTransient<IPersonIndexService, PersonIndexService>();
        
        return sc;
    }
}