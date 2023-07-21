using CustomBlobIndexer.Models;

namespace CustomBlobIndexer.Services;

public interface IOpenAISearchService
{
    Task<OpenAIResponse> QueryAsync(OpenAIRequest request);
}