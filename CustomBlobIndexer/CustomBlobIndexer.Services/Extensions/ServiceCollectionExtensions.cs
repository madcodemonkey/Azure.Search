using Microsoft.Extensions.DependencyInjection;

namespace CustomBlobIndexer.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection sc, ServiceSettings settings)
    {
        sc.AddSingleton(settings);

        sc.AddScoped<IBlobSasBuilderService, BlobSasBuilderService>();

        sc.AddTransient<ICustomComputerVisionService, CustomComputerVisionService>();
        sc.AddTransient<ICustomTextAnalyticsService, CustomTextAnalyticsService>();
        sc.AddTransient<ICustomSearchIndexService, CustomSearchIndexService>();
        sc.AddTransient<ITextChunkingService, TextChunkingService>();

        if (settings.ChunkIntoMultipleDocuments)
        {
            sc.AddTransient<IFileProcessService, FileProcessChunkingService>();
        }
        else
        {
            sc.AddTransient<IFileProcessService, FileProcessService>();
        }

        return sc;
    }
}