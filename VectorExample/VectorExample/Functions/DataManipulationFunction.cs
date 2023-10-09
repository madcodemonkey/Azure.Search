using System.Net;
using VectorExample.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace VectorExample.Functions;

public class DataManipulationFunction
{
    private readonly ICustomDataService _dataService;
    private readonly ILogger _logger;
    private readonly ApplicationSettings _appSettings;
   

    /// <summary>
    /// Constructor
    /// </summary>
    public DataManipulationFunction(ILoggerFactory loggerFactory, 
        IOptions<ApplicationSettings> appSettings,
        ICustomDataService dataService)
    {
        _dataService = dataService;
        _appSettings = appSettings.Value; 
        _logger = loggerFactory.CreateLogger<DataManipulationFunction>();
    }

    /// <summary>
    /// Creates data in the index.
    /// </summary>
    [Function("Data-Creator")]
    public async Task<HttpResponseData> DataCreator(
        [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"C# HTTP trigger function called to create docs in an index named {_appSettings.CognitiveSearchIndexName}.");

        var wasCreated = await _dataService.CreateDocumentsAsync(cancellationToken);
      
        var response =  req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
        string message = wasCreated ? "were" : "were NOT";
        await response.WriteStringAsync($"Documents {message} created inside of an index named {_appSettings.CognitiveSearchIndexName}!", cancellationToken);

        return response;
    }

    /// <summary>
    /// Looks for index documents that need their embeddings update and updates the number specified below.
    /// </summary>
    [Function("Data-Update-Embeddings")]
    public async Task<HttpResponseData> DataEncoder(
        [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req, int batchSize, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"C# HTTP trigger function called to update embeddings in an index named {_appSettings.CognitiveSearchIndexName}.");

        int numberUpdated = await _dataService.UpdateEmbeddingsAsync(batchSize, cancellationToken);

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
        await response.WriteStringAsync($"Updated {numberUpdated} documents in the index named {_appSettings.CognitiveSearchIndexName}!", cancellationToken);

        return response;
    }
}