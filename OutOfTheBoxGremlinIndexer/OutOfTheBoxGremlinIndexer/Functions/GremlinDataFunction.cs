using System.Net;
using CustomSqlServerIndexer.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace OutOfTheBoxGremlinIndexer.Functions;

public class GremlinDataFunction
{
    private readonly IGremlinDataService _dataService;
    private readonly ILogger _logger;

    public GremlinDataFunction(ILoggerFactory loggerFactory, IGremlinDataService dataService)
    {
        _dataService = dataService;
        _logger = loggerFactory.CreateLogger<GremlinDataFunction>();
    }

    [Function("Gremlin-Create-All-Data")]
    public async Task<HttpResponseData> CreateAllDataAsync([HttpTrigger(AuthorizationLevel.Function, "post")] 
        HttpRequestData req, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Request to create all data (this will first delete all data)");


        await _dataService.CreateAllAsync(cancellationToken);


        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
        await response.WriteStringAsync("All data created!", cancellationToken);
        return response;
    }

    [Function("Gremlin-Delete-All-Data")]
    public async Task<HttpResponseData> DeleteAllDataAsync([HttpTrigger(AuthorizationLevel.Function, "post")] 
        HttpRequestData req, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Request to create all data (this will first delete all data)");


        await _dataService.DeleteAllAsync(cancellationToken);


        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
        await response.WriteStringAsync("All data deleted!", cancellationToken);
        return response;
    }

    [Function("Gremlin-List-people")]
    public async Task<HttpResponseData> ListPeopleAsync([HttpTrigger(AuthorizationLevel.Function, "get")]
        HttpRequestData req, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Requesting list of people.");
       
        var result = await _dataService.GetPeopleAsync(cancellationToken);

        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(result, cancellationToken);
        return response;
    }
}