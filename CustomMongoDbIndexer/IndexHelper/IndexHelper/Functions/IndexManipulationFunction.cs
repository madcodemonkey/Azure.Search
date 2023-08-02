using System.Net;
using IndexHelper.Models;
using IndexHelper.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Search.CogServices;


namespace CustomBlobIndexer.Functions;

public class IndexManipulationFunction
{
    private readonly ILogger _logger;
    private readonly IPersonIndexService _personIndexService;
    private readonly IAcmeCogIndexService _cogIndexService;
    private readonly IndexAppSettings _indexAppSettings;
    
    /// <summary>
    /// Constructor
    /// </summary>
    public IndexManipulationFunction(ILoggerFactory loggerFactory, IndexAppSettings indexAppSettings, 
        IPersonIndexService personIndexService, IAcmeCogIndexService cogIndexService)
    {
        _indexAppSettings = indexAppSettings;
        _personIndexService = personIndexService;
        _cogIndexService = cogIndexService;
        _logger = loggerFactory.CreateLogger<IndexManipulationFunction>();
    }

    [Function("Index-Creator")]
    public HttpResponseData IndexCreator([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
    {
        _logger.LogInformation($"C# HTTP trigger function called to create An index named {_indexAppSettings.CognitiveSearchIndexName}.");

        _personIndexService.CreateOrUpdateIndex();

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
        response.WriteString($"An index named {_indexAppSettings.CognitiveSearchIndexName} was created/updated!");

        return response;
    }

    [Function("Index-DocumentDeleter")]
    public async Task<HttpResponseData> IndexDocumentDeleter([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
    {
        _logger.LogInformation($"C# HTTP trigger function called to delete/clean documents in the index named {_indexAppSettings.CognitiveSearchIndexName}.");

        await _cogIndexService.DeleteAllDocumentsAsync(_indexAppSettings.CognitiveSearchIndexName, nameof(PersonIndexDocument.Id));

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
        await response.WriteStringAsync($"All documents in the index named {_indexAppSettings.CognitiveSearchIndexName} were deleted!");

        return response;
    }
}