using Azure;
using Azure.AI.OpenAI;
using Microsoft.Extensions.Options;

namespace VectorExample.Services;

public abstract class OpenAIClientService
{
    protected OpenAiSettings OpenAiSettings { get; }
    private OpenAIClient? _client = null;
    
    /// <summary>
    /// Constructor
    /// </summary>
    protected OpenAIClientService(IOptions<OpenAiSettings> openAiSettings)
    {
        OpenAiSettings = openAiSettings.Value;
    }

    protected OpenAIClient GetClient()
    {
        if (_client == null)
        {
            // Note: The Azure OpenAI client library for .NET is in preview.

            // Install the .NET library via NuGet: dotnet add package Azure.AI.OpenAI --version 1.0.0-beta.6 
            if (string.IsNullOrWhiteSpace(OpenAiSettings.Endpoint) || OpenAiSettings.Endpoint.StartsWith("---"))
                throw new ArgumentException("You must specify a URL for the open ai client!  Please obtain it from the Azure Portal!");
            if (string.IsNullOrWhiteSpace(OpenAiSettings.Key) || OpenAiSettings.Key.StartsWith("---"))
                throw new ArgumentException("You must specify a key for the open ai client!  Please obtain it from the Azure Portal!");

            _client = new OpenAIClient(new Uri(OpenAiSettings.Endpoint), new AzureKeyCredential(OpenAiSettings.Key));
        }

        return _client;
    }
}