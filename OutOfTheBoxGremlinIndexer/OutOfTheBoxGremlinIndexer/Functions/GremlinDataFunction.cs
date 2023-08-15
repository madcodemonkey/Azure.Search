using System.Net;
using Azure.Search.Documents.Models;
using Azure.Search.Documents;
using CustomSqlServerIndexer.Models;
using CustomSqlServerIndexer.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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
    public async Task<HttpResponseData> CreateAllDataAsync([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
    {
        _logger.LogInformation("Request to create all data (this will first delete all data)");


        await _dataService.CreateAllAsync();


        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
        await response.WriteStringAsync("All data created!");
        return response;
    }

    [Function("Gremlin-Delete-All-Data")]
    public async Task<HttpResponseData> DeleteAllDataAsync([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
    {
        _logger.LogInformation("Request to create all data (this will first delete all data)");


        await _dataService.DeleteAllAsync();


        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
        await response.WriteStringAsync("All data deleted!");
        return response;
    }

    [Function("List-people")]
    public async Task<HttpResponseData> ListPeopleAsync([HttpTrigger(AuthorizationLevel.Function, "get")]
        HttpRequestData req)
    {
        _logger.LogInformation("Requesting list of people.");
       
        var result = await _dataService.GetPeopleAsync();

        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(result);
        return response;
    }
}