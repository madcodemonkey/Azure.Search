namespace VectorExample.Services;

public interface IOpenAIService
{
    /// <summary>
    /// Creates an embedding using the model specified in the settings.
    /// </summary>
    /// <param name="text">The text to create an embedding with</param>
    /// <param name="cancellationToken">The cancellation token to stop the process if necessary</param>
    Task<IReadOnlyList<float>> GenerateEmbeddingAsync(string text, CancellationToken cancellationToken = default);

    /// <summary>
    /// The current embedding version number
    /// </summary>
    int EmbeddingVersion { get; set; }
}