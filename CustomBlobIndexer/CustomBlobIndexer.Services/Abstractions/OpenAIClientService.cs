using Azure;
using Azure.AI.OpenAI;

namespace CustomBlobIndexer.Services;

public abstract class OpenAIClientService
{
    protected ServiceSettings Settings { get; }
    private OpenAIClient? _client = null;
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="settings"></param>
    protected OpenAIClientService(ServiceSettings settings)
    {
        Settings = settings;
    }

    protected OpenAIClient GetClient()
    {
        if (_client == null)
        {
            // Note: The Azure OpenAI client library for .NET is in preview.

            // Install the .NET library via NuGet: dotnet add package Azure.AI.OpenAI --version 1.0.0-beta.6 
            if (string.IsNullOrWhiteSpace(Settings.OpenAIEndpoint) || Settings.OpenAIEndpoint.StartsWith("---"))
                throw new ArgumentException("You must specify a URL for the open ai client!  Please obtain it from the Azure Portal!");
            if (string.IsNullOrWhiteSpace(Settings.OpenAIKey) || Settings.OpenAIKey.StartsWith("---"))
                throw new ArgumentException("You must specify a key for the open ai client!  Please obtain it from the Azure Portal!");

            _client = new OpenAIClient(new Uri(Settings.OpenAIEndpoint), new AzureKeyCredential(Settings.OpenAIKey));
        }

        return _client;
    }
}