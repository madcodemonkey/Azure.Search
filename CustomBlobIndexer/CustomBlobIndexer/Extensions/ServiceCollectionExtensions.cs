using CustomBlobIndexer.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CustomBlobIndexer;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomBlobIndexerDependencies(this IServiceCollection sc, IConfiguration config)
    {
        // Warning!! Use of  Environment.GetEnvironmentVariable("RunInformation") will not currently get you secrets.json overrides; thus, I'm injecting IConfiguration will does have the overrides!
        var serviceSettings = new ServiceSettings
        {
            BlobAccessKey = config["BlobAccessKey"],
            BlobAccountName = config["BlobAccountName"],
            BlobContainerName = config["BlobContainerName"],
            ChunkIntoMultipleDocuments = bool.Parse(config["ChunkIntoMultipleDocuments"] ?? "true"),
            ChunkMaximumNumberOfCharacters = int.Parse(config["ChunkMaximumNumberOfCharacters"] ?? "1000"),
            CognitiveSearchIndexName = config["CognitiveSearchIndexName"],
            CognitiveSearchKey = config["CognitiveSearchKey"],
            CognitiveSearchName = config["CognitiveSearchName"],
            CognitiveSearchSemanticConfigurationName = config["CognitiveSearchSemanticConfigurationName"],
            CognitiveSearchSkillDetectEntities = bool.Parse(config["CognitiveSearchSkillDetectEntities"] ?? "true"),
            CognitiveSearchSkillDetectKeyPhrases = bool.Parse(config["CognitiveSearchSkillDetectKeyPhrases"] ?? "true"),
            CognitiveSearchSkillDetectLanguage = bool.Parse(config["CognitiveSearchSkillDetectLanguage"] ?? "true"),
            CognitiveSearchSkillDetectSentiment = bool.Parse(config["CognitiveSearchSkillDetectSentiment"] ?? "false"),
            CognitiveSearchSkillRedactText = bool.Parse(config["CognitiveSearchSkillRedactText"] ?? "false"),
            CognitiveServiceEndpoint = config["CognitiveServiceEndpoint"],
            CognitiveServiceKey = config["CognitiveServiceKey"],
            OpenAIDeploymentOrModelName = config["OpenAIDeploymentOrModelName"],
            OpenAIEndpoint = config["OpenAIEndpoint"],
            OpenAIKey = config["OpenAIKey"]
        };

        sc.AddServices(serviceSettings);

        return sc;
    }
}