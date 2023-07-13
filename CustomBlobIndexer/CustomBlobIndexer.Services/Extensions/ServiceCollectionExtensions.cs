using Microsoft.Extensions.DependencyInjection;

namespace CustomBlobIndexer.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection sc, ServiceSettings settings)
    {
        sc.AddSingleton(settings);

        sc.AddScoped<IBlobSasBuilderService, BlobSasBuilderService>();
        
        return sc;
    }
}