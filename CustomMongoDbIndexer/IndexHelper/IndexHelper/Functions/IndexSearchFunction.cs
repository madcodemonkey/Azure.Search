using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using IndexHelper.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;
using CogSearchServices.Models;

namespace CustomBlobIndexer.Functions;

/// <summary>
/// Used search the Cognitive Search index.
/// </summary>
public class IndexSearchFunction
{
    private readonly IPersonIndexService _searchIndexService;
    private readonly ILogger _logger;

    /// <summary>
    /// Constructor
    /// </summary>
    public IndexSearchFunction(ILoggerFactory loggerFactory, 
     
        IPersonIndexService searchIndexService)
    {
        _logger = loggerFactory.CreateLogger<IndexSearchFunction>();
        _searchIndexService = searchIndexService;
    }

    [Function("Index-SimpleSearch")]
    public async Task<HttpResponseData> SimpleSearch([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        CogSearchRequest? data = JsonConvert.DeserializeObject<CogSearchRequest>(requestBody);

        var result = data != null
            ? await _searchIndexService.SearchAsync<SearchDocument>(data.Query, new SearchOptions()
            {
                IncludeTotalCount = data.IncludeCount,
                QueryType = SearchQueryType.Simple,
                SearchMode = data.IncludeAllWords ? SearchMode.All : SearchMode.Any
            })
            : new CogSearchQueryResponse<SearchDocument>();

        
        var response = req.CreateResponse(HttpStatusCode.OK); 
        await response.WriteAsJsonAsync(result);
        return response;
    }
}