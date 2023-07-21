using System.Text;
using Azure.AI.OpenAI;
using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using CustomBlobIndexer.Models;

namespace CustomBlobIndexer.Services;

public class OpenAISearchService : OpenAIClientService, IOpenAISearchService
{
    private readonly ICustomSearchIndexService _searchIndexService;

    /// <summary>
    /// Constructor
    /// </summary>
    public OpenAISearchService(ServiceSettings settings, ICustomSearchIndexService searchIndexService) : base(settings)
    {
        _searchIndexService = searchIndexService;
    }

    public async Task<OpenAIResponse> QueryAsync(OpenAIRequest request)
    {
        var cognitiveSearchOptions = new SearchOptions()
        {
            IncludeTotalCount = true,
            QueryType = SearchQueryType.Semantic,
            QueryLanguage = QueryLanguage.EnUs,
            SemanticConfigurationName = Settings.CognitiveSearchSemanticConfigurationName
        };

        var response = await _searchIndexService.SearchAsync<SearchDocument>(request.Query, cognitiveSearchOptions);

        if (response.TotalCount == 0)
        {
            return new OpenAIResponse { Answer = "I don't know the answer to that question because Cognitive Search didn't provide any documents!" };
        }

        StringBuilder sbPrompt = new StringBuilder();
        sbPrompt.AppendLine("You are an AI assistant that helps people find information using this data try to answer the question.");
        foreach (SearchResult<SearchDocument> doc in response.Docs)
        {
            sbPrompt.AppendLine(doc.Document[nameof(SearchIndexDocument.Content)].ToString());
        }
        sbPrompt.AppendLine(request.Query);

        
        var client = GetClient();

        Response<Completions> completionsResponse =
            await client.GetCompletionsAsync(Settings.OpenAIDeploymentOrModelName,
                new CompletionsOptions()
                {
                    Temperature = (float)0.7,
                    MaxTokens = 800,
                    Prompts = { sbPrompt.ToString() },
                    NucleusSamplingFactor = (float)0.95,
                });
        
        // Console.WriteLine($"Open API responded with {completionsResponse.Value.Choices.Count} choices");

        string completion = completionsResponse.Value.Choices.Count > 0 ? 
            completionsResponse.Value.Choices[0].Text : 
            "I don't know.";

        return new OpenAIResponse { Answer =  completion};
    }
}