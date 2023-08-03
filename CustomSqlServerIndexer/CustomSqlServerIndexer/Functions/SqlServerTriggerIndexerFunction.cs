using System.Net;
using CustomSqlServerIndexer.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace CustomSqlServerIndexer.Functions
{
    public class SqlServerTriggerIndexerFunction
    {
        private readonly ICustomSqlServerIndexerService _indexerService;
        private readonly ILogger _logger;

        public SqlServerTriggerIndexerFunction(ILoggerFactory loggerFactory, ICustomSqlServerIndexerService indexerService)
        {
            _indexerService = indexerService;
            _logger = loggerFactory.CreateLogger<SqlServerTriggerIndexerFunction>();
        }

        [Function("SqlServerTriggerIndexerFunction")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req, CancellationToken cancellationToken)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            int cogSearchIndexChangeCount = await _indexerService.DoWorkAsync(cancellationToken);
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            await response.WriteStringAsync($"Manual Trigger made {cogSearchIndexChangeCount} changes to the Cognitive Search index!", cancellationToken);
            
            return response;
        }
    }
}
