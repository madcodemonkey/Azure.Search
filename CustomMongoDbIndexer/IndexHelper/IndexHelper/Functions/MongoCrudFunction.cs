using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace IndexHelper.Functions;

/// <summary>
/// Used to Create, Update and Delete MongoDB records.  Reading should be done via the Azure Search endpoints.
/// </summary>
public class MongoCrudFunction
{
    private readonly ILogger _logger;

    /// <summary>
    /// Constructor
    /// </summary>
    public MongoCrudFunction(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<MongoCrudFunction>();
    }
    
    [Function("Mongo-Create")]
    public async Task<HttpResponseData> CreateOneAsync([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
    {
        _logger.LogInformation("Requesting a document be created.");

        //string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        //SearchRequest? data = JsonConvert.DeserializeObject<SearchRequest>(requestBody);
        
        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
        response.WriteString("Document was created!");
        return response;
    }

    [Function("Mongo-Delete")]
    public async Task<HttpResponseData> DeleteOneAsync([HttpTrigger(AuthorizationLevel.Function, "delete")] HttpRequestData req)
    {
        _logger.LogInformation("Requesting a document be created.");



        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
        response.WriteString("Document was deleted!");
        return response;
    }
    
    [Function("Mongo-Update")]
    public async Task<HttpResponseData> UpdateOneAsync([HttpTrigger(AuthorizationLevel.Function, "put")] HttpRequestData req)
    {
        _logger.LogInformation("Requesting a document be updated.");

        //string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        //SearchRequest? data = JsonConvert.DeserializeObject<SearchRequest>(requestBody);

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
        response.WriteString("Document was updated!");
        return response;
    }
}