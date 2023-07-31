using System.Net;
using IndexHelper.Models;
using IndexHelper.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace IndexHelper.Functions;

/// <summary>
/// Used to Create, Update and Delete MongoDB records.  Reading should be done via the Azure Search endpoints.
/// </summary>
public class MongoCrudFunction
{
    private readonly IPersonMongoService _mongoService;
    private readonly ILogger _logger;

    /// <summary>
    /// Constructor
    /// </summary>
    public MongoCrudFunction(ILoggerFactory loggerFactory, IPersonMongoService mongoService)
    {
        _mongoService = mongoService;
        _logger = loggerFactory.CreateLogger<MongoCrudFunction>();
    }
    
    [Function("Mongo-Create")]
    public async Task<HttpResponseData> CreateOneAsync([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Requesting a document be created.");

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var person = JsonConvert.DeserializeObject<PersonModel>(requestBody);

        if (person == null)
        {
            return req.CreateResponse(HttpStatusCode.BadRequest);
        }

        await _mongoService.CreateAsync(person, cancellationToken);

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
        await response.WriteStringAsync("Document was created!", cancellationToken);
        return response;
    }

    [Function("Mongo-Delete")]
    public async Task<HttpResponseData> DeleteOneAsync([HttpTrigger(AuthorizationLevel.Function, "delete")] HttpRequestData req,
        string personId, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Requesting a document be created.");

        await _mongoService.DeleteAsync(personId, cancellationToken);

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
        await response.WriteStringAsync("Document was deleted!", cancellationToken);
        return response;
    }

    [Function("Mongo-GetAll")]
    public async Task<HttpResponseData> GetAllAsync([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req,
         int pageNumber, int itemsPerPage, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Requesting all documents.");
        
        var result =  await _mongoService.GetAllAsync(pageNumber, itemsPerPage, cancellationToken);

        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(result,cancellationToken);
        return response;
    }

    [Function("Mongo-Update")]
    public async Task<HttpResponseData> UpdateOneAsync([HttpTrigger(AuthorizationLevel.Function, "put")] HttpRequestData req, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Requesting a document be updated.");

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var person = JsonConvert.DeserializeObject<PersonModel>(requestBody);

        if (person == null)
        {
            return req.CreateResponse(HttpStatusCode.BadRequest);
        }

        await _mongoService.UpdateAsync(person, cancellationToken);

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
        await response.WriteStringAsync("Document was updated!", cancellationToken);
        return response;
    }
}