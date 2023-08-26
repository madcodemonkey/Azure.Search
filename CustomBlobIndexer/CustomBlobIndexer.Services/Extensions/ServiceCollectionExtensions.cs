using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CustomBlobIndexer.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection sc, IConfiguration config)
    {
        var applicationSettings = ConfigureSectionSettings<ApplicationSettings>(sc, config, ApplicationSettings.SectionName);
        ConfigureSectionSettings<BlobSettings>(sc, config, BlobSettings.SectionName);
        ConfigureSectionSettings<OpenAiSettings>(sc, config, OpenAiSettings.SectionName, checkProperties: false); // These are optional
        ConfigureSectionSettings<CognitiveServiceSettings>(sc, config, CognitiveServiceSettings.SectionName);
      
        sc.AddScoped<IBlobSasBuilderService, BlobSasBuilderService>();

        sc.AddTransient<ICustomComputerVisionService, CustomComputerVisionService>();
        sc.AddTransient<ICustomIndexService, CustomIndexService>();
        sc.AddTransient<ICustomTextAnalyticsService, CustomTextAnalyticsService>();
        sc.AddTransient<IOpenAISearchService, OpenAISearchService>();
        sc.AddTransient<ITextChunkingService, TextChunkingService>();

        
        if (applicationSettings.ChunkIntoMultipleDocuments)
        {
            sc.AddTransient<IFileProcessService, FileProcessChunkingService>();
        }
        else
        {
            sc.AddTransient<IFileProcessService, FileProcessService>();
        }

        return sc;
    }


    private static T ConfigureSectionSettings<T>(IServiceCollection sc, IConfiguration config, string sectionName, bool checkProperties = true) where T : class, new()
    {
        var setting = new T();
        config.GetSection(sectionName).Bind(setting);

        if (checkProperties)
        {
            foreach (PropertyInfo propertyInfo in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (propertyInfo.PropertyType == typeof(string))
                {
                    var someString = propertyInfo.GetValue(setting) as string;
                    if (string.IsNullOrWhiteSpace(someString) || someString.StartsWith("---"))
                    {
                        throw new Exception($"Please add the {sectionName}:{propertyInfo.Name} to your " +
                                            $"local.settings.json files, secrets file or if deployed your configuration settings!");
                    }
                }
            }
        }

        sc.Configure<T>(config.GetSection(sectionName));

        return setting;
    }
}