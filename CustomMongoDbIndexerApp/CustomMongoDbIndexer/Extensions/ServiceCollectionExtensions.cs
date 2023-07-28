using CustomMongoDbIndexer.Services;
using CustomMongoDbIndexer.Services.Extensions;

namespace CustomMongoDbIndexer;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomDependencies(this IServiceCollection sc, IConfiguration config)
    {
        var applicationSettings = new ApplicationSettings();

        config.GetSection("AppSettings").Bind(applicationSettings);
        
        sc.AddServices(applicationSettings);

        return sc;
    }
}