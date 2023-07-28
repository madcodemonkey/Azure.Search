using IndexHelper.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace IndexHelper.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection sc)
    {
        sc.AddTransient<IPersonRepository, PersonRepository>();
        
        return sc;
    }
}