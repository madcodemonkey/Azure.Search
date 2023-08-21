using System.Net;
using CustomSqlServerIndexer.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using OutOfTheBoxGremlinIndexer.Services;

namespace CustomSqlServerIndexer.Functions;

public class IndexManipulationFunction
{
    private readonly ILogger _logger;
    private readonly ServiceSettings _settings;
    private readonly IOutOfBoxService _outOfBoxService;

    /// <summary>
    /// Constructor
    /// </summary>
    public IndexManipulationFunction(ILoggerFactory loggerFactory, 
        ServiceSettings settings, IOutOfBoxService outOfBoxService)
    {
        _settings = settings;
        _outOfBoxService = outOfBoxService;
        _logger = loggerFactory.CreateLogger<IndexManipulationFunction>();
    }

    [Function("Index-Creator")]
    public async Task<HttpResponseData> IndexCreatorAsync([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
    {
        _logger.LogInformation($"C# HTTP trigger function called to create An index named {_settings.CognitiveSearchIndexName}.");

        await _outOfBoxService.CreateAsync();

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
        await response.WriteStringAsync($"An index named {_settings.CognitiveSearchIndexName} was created/updated!");

        return response;
    }

    [Function("Index-Deleter")]
    public async Task<HttpResponseData> IndexDeleteAsync(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation($"C# HTTP trigger function called to delete an index named {_settings.CognitiveSearchIndexName}.");

        await _outOfBoxService.DeleteAsync(cancellationToken);

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
        await response.WriteStringAsync($"An index named {_settings.CognitiveSearchIndexName} was deleted!", cancellationToken);

        return response;
    }


    [Function("Indexer-Run")]
    public async Task<HttpResponseData> IndexerRunAsync(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation($"C# HTTP trigger function called to run the indexER named {_settings.CognitiveSearchIndexerName}.");

        await _outOfBoxService.RunIndexerAsync(cancellationToken);

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
        await response.WriteStringAsync($"The indexer named {_settings.CognitiveSearchIndexName} was told to run!", cancellationToken);

        return response;
    }
}