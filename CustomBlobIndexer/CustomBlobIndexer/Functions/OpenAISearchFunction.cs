using System.Net;
using CustomBlobIndexer.Models;
using CustomBlobIndexer.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CustomBlobIndexer.Functions;

public class OpenAISearchFunction
{
    private readonly IOpenAISearchService _openAISearchService;

    private readonly ILogger _logger;

    /// <summary>
    /// Constructor
    /// </summary>
    public OpenAISearchFunction(ILoggerFactory loggerFactory,
        IOpenAISearchService openAISearchService)
    {
        _openAISearchService = openAISearchService;
        _logger = loggerFactory.CreateLogger<OpenAISearchFunction>();
    }

    [Function("OpenAISearch")]
    public async Task<HttpResponseData> OpenAISearch([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req, CancellationToken cancellationToken)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var data = JsonConvert.DeserializeObject<OpenAIRequest>(requestBody);

        OpenAIResponse openAIResponse;

        if (data != null)
        {
            if (string.IsNullOrWhiteSpace(data.SituationStatement))
                data.SituationStatement = "You are an AI assistant that helps people find information using this data try to answer the question.";

            if (string.IsNullOrWhiteSpace(data.SearchFieldName))
                data.SearchFieldName = nameof(SearchIndexDocument.Content);

            openAIResponse = await _openAISearchService.QueryAsync(data, cancellationToken);
        }
        else
        {
            openAIResponse = new OpenAIResponse { Answer = "Unable to understand your request object!" };
        }
        
        
        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(openAIResponse, cancellationToken);
        return response;
    }
}