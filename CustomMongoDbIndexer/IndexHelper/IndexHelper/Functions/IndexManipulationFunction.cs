using System.Net;
using CogSearchServices.Services;
using IndexHelper.Models;
using IndexHelper.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
 

namespace CustomBlobIndexer.Functions;

public class IndexManipulationFunction
{
    private readonly ILogger _logger;
    private readonly IPersonIndexService _searchIndexService;
    private readonly CogSearchServiceSettings _cogSettings;

    /// <summary>
    /// Constructor
    /// </summary>
    public IndexManipulationFunction(ILoggerFactory loggerFactory, CogSearchServiceSettings cogSettings,
        IPersonIndexService searchIndexService)
    {
        _cogSettings = cogSettings;
        _searchIndexService = searchIndexService;
        _logger = loggerFactory.CreateLogger<IndexManipulationFunction>();
    }

    [Function("Index-Creator")]
    public HttpResponseData IndexCreator([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
    {
        _logger.LogInformation($"C# HTTP trigger function called to create An index named {_cogSettings.CognitiveSearchIndexName}.");

        _searchIndexService.CreateOrUpdateIndex();

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
        response.WriteString($"An index named {_cogSettings.CognitiveSearchIndexName} was created/updated!");

        return response;
    }

    [Function("Index-DocumentDeleter")]
    public async Task<HttpResponseData> IndexDocumentDeleter([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
    {
        _logger.LogInformation($"C# HTTP trigger function called to delete/clean documents in the index named {_cogSettings.CognitiveSearchIndexName}.");

        await _searchIndexService.DeleteAllDocumentsAsync(nameof(PersonIndexDocument.Id));

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
        response.WriteString($"All documents in the index named {_cogSettings.CognitiveSearchIndexName} were deleted!");

        return response;
    }
}