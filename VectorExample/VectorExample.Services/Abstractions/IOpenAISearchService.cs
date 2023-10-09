using VectorExample.Models;

namespace VectorExample.Services;

public interface IOpenAISearchService
{
    /// <summary>
    /// Queries the Cognitive Search endpoint using the semantic option and then using the
    /// results to query Open AI.
    /// </summary>
    /// <param name="request">The request</param>
    /// <param name="cancellationToken"></param>
    Task<OpenAIResponse> QueryAsync(OpenAIRequest request, CancellationToken cancellationToken);
}