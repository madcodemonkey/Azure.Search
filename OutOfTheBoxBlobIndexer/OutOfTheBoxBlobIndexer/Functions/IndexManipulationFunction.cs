using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using OutOfTheBoxBlobIndexer.Models;
using OutOfTheBoxBlobIndexer.Services;
using OutOfTheBoxBlobIndexer.Services.Services;

namespace OutOfTheBoxBlobIndexer.Functions;

public class IndexManipulationFunction
{
    private readonly ILogger _logger;
    private readonly ICustomSearchIndexService _searchIndexService;
    private readonly ServiceSettings _settings;
    private readonly IOutOfBoxIndexService _outOfBoxIndexService;

    /// <summary>
    /// Constructor
    /// </summary>
    public IndexManipulationFunction(ILoggerFactory loggerFactory, 
        ServiceSettings settings, IOutOfBoxIndexService outOfBoxIndexService,
        ICustomSearchIndexService searchIndexService)
    {
        _settings = settings;
        _outOfBoxIndexService = outOfBoxIndexService;
        _searchIndexService = searchIndexService;
        _logger = loggerFactory.CreateLogger<IndexManipulationFunction>();
    }

    [Function("IndexCreator")]
    public async Task<HttpResponseData> IndexCreator([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
    {
        _logger.LogInformation($"C# HTTP trigger function called to create An index named {_settings.CognitiveSearchIndexName}.");

        await _outOfBoxIndexService.CreateAsync();

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
        await response.WriteStringAsync($"An index named {_settings.CognitiveSearchIndexName} was created/updated!");

        return response;
    }

    [Function("IndexDeleter")]
    public async Task<HttpResponseData> IndexDelete([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
    {
        _logger.LogInformation($"C# HTTP trigger function called to delete an index named {_settings.CognitiveSearchIndexName}.");

        await _searchIndexService.DeleteIndexAsync();

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
        await response.WriteStringAsync($"An index named {_settings.CognitiveSearchIndexName} was deleted!");

        return response;
    }

    [Function("IndexDocumentDeleter")]
    public async Task<HttpResponseData> IndexDocumentDeleter([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
    {
        _logger.LogInformation($"C# HTTP trigger function called to delete/clean documents in the index named {_settings.CognitiveSearchIndexName}.");

        await _searchIndexService.DeleteAllDocumentsAsync(nameof(SearchIndexDocument.Id));

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
        await response.WriteStringAsync($"All documents in the index named {_settings.CognitiveSearchIndexName} were deleted!");

        return response;
    }
}