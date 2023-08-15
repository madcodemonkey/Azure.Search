using System.Net;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OutOfTheBoxBlobIndexer.Models;
using OutOfTheBoxBlobIndexer.Services;

namespace OutOfTheBoxBlobIndexer.Functions;

public class IndexSearchFunction
{
    private readonly ServiceSettings _settings;
    private readonly ICogSearchIndexService _searchIndexService;
    private readonly ILogger _logger;

    /// <summary>
    /// Constructor
    /// </summary>
    public IndexSearchFunction(ILoggerFactory loggerFactory,
        ServiceSettings settings,
        ICogSearchIndexService searchIndexService)
    {
        _logger = loggerFactory.CreateLogger<IndexSearchFunction>();
        _settings = settings;
        _searchIndexService = searchIndexService;
    }

    [Function("IndexSearch")]
    public async Task<HttpResponseData> IndexSearch([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        SearchRequest? data = JsonConvert.DeserializeObject<SearchRequest>(requestBody);

        var result = data != null
            ? await _searchIndexService.SearchAsync<SearchDocument>(_settings.CognitiveSearchIndexName, data.Query, new SearchOptions()
            {
                IncludeTotalCount = data.IncludeCount,
                Size = data.PageSize,
                Skip = (data.PageNumber - 1) * data.PageSize,
                QueryType = SearchQueryType.Simple,
                SearchMode = data.IncludeAllWords ? SearchMode.All : SearchMode.Any
            })
            : new SearchQueryResponse<SearchDocument>();

        
        var response = req.CreateResponse(HttpStatusCode.OK); 
        await response.WriteAsJsonAsync(result);
        return response;
    }
}