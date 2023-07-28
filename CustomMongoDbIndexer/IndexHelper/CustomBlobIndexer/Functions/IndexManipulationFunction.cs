using System.Net;
using IndexHelper.Models;
using IndexHelper.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using SearchServices.Services;

namespace CustomBlobIndexer.Functions;

public class IndexManipulationFunction
{
    private readonly ILogger _logger;
    private readonly IPersonIndexService _searchIndexService;
    private readonly SearchServiceSettings _settings;

    /// <summary>
    /// Constructor
    /// </summary>
    public IndexManipulationFunction(ILoggerFactory loggerFactory, SearchServiceSettings settings,
        IPersonIndexService searchIndexService)
    {
        _settings = settings;
        _searchIndexService = searchIndexService;
        _logger = loggerFactory.CreateLogger<IndexManipulationFunction>();
    }

    [Function("IndexCreator")]
    public HttpResponseData IndexCreator([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
    {
        _logger.LogInformation($"C# HTTP trigger function called to create An index named {_settings.CognitiveSearchIndexName}.");

        _searchIndexService.CreateOrUpdateIndex();

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
        response.WriteString($"An index named {_settings.CognitiveSearchIndexName} was created/updated!");

        return response;
    }

    [Function("IndexDocumentDeleter")]
    public async Task<HttpResponseData> IndexDocumentDeleter([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
    {
        _logger.LogInformation($"C# HTTP trigger function called to delete/clean documents in the index named {_settings.CognitiveSearchIndexName}.");

        await _searchIndexService.DeleteAllDocumentsAsync(nameof(SearchIndexDocument.Id));

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
        response.WriteString($"All documents in the index named {_settings.CognitiveSearchIndexName} were deleted!");

        return response;
    }
}