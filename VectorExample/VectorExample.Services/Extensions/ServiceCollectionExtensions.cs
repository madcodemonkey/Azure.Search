using CogSimple.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace VectorExample.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection sc, IConfiguration config)
    {
        sc.ConfigureSectionSettings<ApplicationSettings>(config, ApplicationSettings.SectionName);
        sc.ConfigureSectionSettings<OpenAiSettings>(config, OpenAiSettings.SectionName, checkProperties: false); 

        sc.AddScoped<ICustomDataService, CustomDataService>();
        sc.AddScoped<ICustomIndexService, CustomIndexService>();
        sc.AddScoped<IOpenAIService, OpenAIService>();

        return sc;
    }
}