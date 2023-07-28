using Microsoft.Extensions.DependencyInjection;

namespace MongoDBServices;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCommonMongoDBServices(this IServiceCollection sc, MongoDBServiceSettings settings)
    {
        sc.AddSingleton(settings);

        sc.AddSingleton<IMongoClientService, MongoClientService>();
        
        return sc;
    }
}