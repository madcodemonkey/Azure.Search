using Azure;
using Azure.AI.OpenAI;
using Microsoft.Extensions.Options;

namespace VectorExample.Services;

public class OpenAIService : IOpenAIService
{
    private readonly OpenAiSettings _settings;
    private OpenAIClient? _client = null;

    public OpenAIService(IOptions<OpenAiSettings>  settings)
    {
        _settings = settings.Value;
    }

    /// <summary>
    /// Creates an embedding using the model specified in the settings.
    /// </summary>
    /// <param name="text">The text to create an embedding with</param>
    /// <param name="cancellationToken">The cancellation token to stop the process if necessary</param>
    public async Task<IReadOnlyList<float>> GenerateEmbeddingAsync(string text, CancellationToken cancellationToken = default)
    {
        OpenAIClient client = GetClient();

        var result = await client.GetEmbeddingsAsync(_settings.DeploymentOrModelName, new EmbeddingsOptions(text), cancellationToken);

        return result.Value.Data[0].Embedding;
    }

    public int EmbeddingVersion { get; set; } = 1;

    /// <summary>
    /// Creates a client.
    /// </summary>
    private OpenAIClient GetClient()
    {
        if (_client == null)
        {
            if (string.IsNullOrWhiteSpace(_settings.Endpoint) || _settings.Endpoint.StartsWith("---"))
            {
                throw new ArgumentException("Please specify a URL endpoint for the open AI client!  You can obtain it from the Azure Portal.");
            }

            if (string.IsNullOrWhiteSpace(_settings.Key) || _settings.Endpoint.StartsWith("---"))
            {
                throw new ArgumentException("Please specify a KEY for the open AI client!  You can obtain it from the Azure Portal.");
            }

            _client = new OpenAIClient(new Uri(_settings.Endpoint), new AzureKeyCredential(_settings.Key));
        }

        return _client;
    }
}