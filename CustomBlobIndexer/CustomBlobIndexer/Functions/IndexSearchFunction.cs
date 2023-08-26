using System.Net;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using CogSimple.Services;
using CustomBlobIndexer.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace CustomBlobIndexer.Functions;

public class IndexSearchFunction
{
    private readonly ApplicationSettings _appSettings;
    private readonly ICogSearchIndexService _cogSearchIndexService;
    private readonly ILogger _logger;

    /// <summary>
    /// Constructor
    /// </summary>
    public IndexSearchFunction(ILoggerFactory loggerFactory,
        IOptions<ApplicationSettings> appSettings,
        ICogSearchIndexService cogSearchIndexService)
    {
        _appSettings = appSettings.Value;
        _cogSearchIndexService = cogSearchIndexService;
        _logger = loggerFactory.CreateLogger<IndexSearchFunction>();
    }

    [Function("IndexSearch")]
    public async Task<HttpResponseData> IndexSearch([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req, CancellationToken cancellationToken)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        SearchRequest? data = JsonConvert.DeserializeObject<SearchRequest>(requestBody);

        var result = data != null
            ? await _cogSearchIndexService.SearchAsync<SearchDocument>(_appSettings.CognitiveSearchIndexName, data.Query, new SearchOptions()
            {
                IncludeTotalCount = data.IncludeCount,
                QueryType = SearchQueryType.Simple,
                SearchMode = data.IncludeAllWords ? SearchMode.All : SearchMode.Any
            }, cancellationToken)
            : new SearchQueryResponse<SearchDocument>();

        
        var response = req.CreateResponse(HttpStatusCode.OK); 
        await response.WriteAsJsonAsync(result, cancellationToken);
        return response;
    }
}