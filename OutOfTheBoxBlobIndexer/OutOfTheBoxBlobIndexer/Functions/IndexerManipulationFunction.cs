using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using OutOfTheBoxBlobIndexer.Services;
using OutOfTheBoxBlobIndexer.Services.Services;

namespace OutOfTheBoxBlobIndexer.Functions
{
    public class IndexerManipulationFunction
    {
        private readonly ServiceSettings _settings;
        private readonly IOutOfBoxService _outOfBoxService;
        private readonly ILogger _logger;

        public IndexerManipulationFunction(ILoggerFactory loggerFactory, ServiceSettings settings, IOutOfBoxService outOfBoxService)
        {
            _settings = settings;
            _outOfBoxService = outOfBoxService;
            _logger = loggerFactory.CreateLogger<IndexerManipulationFunction>();
        }

        [Function("Indexer-Run")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"C# HTTP trigger function called to run the indexer for the index named {_settings.CognitiveSearchIndexName}.");

            await _outOfBoxService.RunIndexerAsync(cancellationToken);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            await response.WriteStringAsync("Indexer is running!", cancellationToken);

            return response;
        }
    }
}
