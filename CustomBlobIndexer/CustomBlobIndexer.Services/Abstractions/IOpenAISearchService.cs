using CustomBlobIndexer.Models;

namespace CustomBlobIndexer.Services;

public interface IOpenAISearchService
{
    /// <summary>
    /// Queries the Cognitive Search endpoint using the semantic option and then using the
    /// results to query Open AI.
    /// </summary>
    /// <param name="request">The request</param>
    Task<OpenAIResponse> QueryAsync(OpenAIRequest request);
}