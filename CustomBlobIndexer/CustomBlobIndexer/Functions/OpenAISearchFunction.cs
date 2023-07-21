using System.Net;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;
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
    public async Task<HttpResponseData> OpenAISearch([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var data = JsonConvert.DeserializeObject<OpenAIRequest>(requestBody);

        var result = data != null
            ? await _openAISearchService.QueryAsync(data)
            : new OpenAIResponse { Answer = "Unable to understand your request object!" };
        
        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(result);
        return response;
    }
}