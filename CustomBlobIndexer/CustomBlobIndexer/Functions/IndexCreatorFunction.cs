using System.Net;
using CustomBlobIndexer.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace CustomBlobIndexer.Functions
{
    public class IndexCreatorFunction
    {
        private readonly ServiceSettings _settings;
        private readonly ICustomSearchIndexService _searchIndexService;
        private readonly ILogger _logger;

        public IndexCreatorFunction(ILoggerFactory loggerFactory, 
            ServiceSettings settings,
            ICustomSearchIndexService searchIndexService)
        {
            _settings = settings;
            _searchIndexService = searchIndexService;
            _logger = loggerFactory.CreateLogger<IndexCreatorFunction>();
        }

        [Function("IndexCreatorFunction")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation($"C# HTTP trigger function called to create An index named {_settings.CognitiveSearchIndexName}.");

            _searchIndexService.CreateOrUpdateIndex();

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            response.WriteString($"An index named {_settings.CognitiveSearchIndexName} was created/updated!");

            return response;
        }
    }
}
