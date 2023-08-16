using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using OutOfTheBoxBlobIndexer.Services;

namespace OutOfTheBoxBlobIndexer.Functions;

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

    [Function("IndexCreator")]
    public async Task<HttpResponseData> IndexCreator([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"C# HTTP trigger function called to create An index named {_settings.CognitiveSearchIndexName}.");

        await _outOfBoxService.CreateAsync(cancellationToken);

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
        await response.WriteStringAsync($"An index named {_settings.CognitiveSearchIndexName} was created/updated!", cancellationToken);

        return response;
    }

    [Function("IndexDeleter")]
    public async Task<HttpResponseData> IndexDelete([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"C# HTTP trigger function called to delete an index named {_settings.CognitiveSearchIndexName}.");

        await _outOfBoxService.DeleteAsync(cancellationToken);

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
        await response.WriteStringAsync($"An index named {_settings.CognitiveSearchIndexName} was deleted!", cancellationToken);

        return response;
    }
}