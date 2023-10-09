using System.Net;
using VectorExample.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace VectorExample.Functions;

public class IndexManipulationFunction
{
    private readonly ILogger _logger;
    private readonly ApplicationSettings _appSettings;
    private readonly ICustomIndexService _customIndexService;

    /// <summary>
    /// Constructor
    /// </summary>
    public IndexManipulationFunction(ILoggerFactory loggerFactory, 
        IOptions<ApplicationSettings> appSettings,
        ICustomIndexService customIndexService)
    {
        _appSettings = appSettings.Value;
        _customIndexService = customIndexService;
        _logger = loggerFactory.CreateLogger<IndexManipulationFunction>();
    }

    [Function("Index-Creator")]
    public async Task<HttpResponseData> IndexCreator(
        [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"C# HTTP trigger function called to create An index named {_appSettings.CognitiveSearchIndexName}.");

        await _customIndexService.CreateAsync(cancellationToken);

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
        await response.WriteStringAsync($"An index named {_appSettings.CognitiveSearchIndexName} was created/updated!", cancellationToken);

        return response;
    }

    [Function("Index-Deleter")]
    public async Task<HttpResponseData> IndexDocumentDeleter(
        [HttpTrigger(AuthorizationLevel.Function, "delete")] HttpRequestData req, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"C# HTTP trigger function called to delete the index named {_appSettings.CognitiveSearchIndexName}.");

        await _customIndexService.DeleteAsync(cancellationToken);

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
        await response.WriteStringAsync($"The index named {_appSettings.CognitiveSearchIndexName} was deleted!", cancellationToken);

        return response;
    }
}