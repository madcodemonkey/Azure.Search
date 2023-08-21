using System.Net;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using CogSimple.Services;
using CustomSqlServerIndexer.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CustomSqlServerIndexer.Functions;

public class IndexSearchFunction
{
    private readonly ServiceSettings _settings;
    private readonly ILogger _logger;
    private readonly ICogSearchIndexService _searchIndexService;

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
    public async Task<HttpResponseData> IndexSearch([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req,
        CancellationToken cancellationToken)
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
            }, cancellationToken)
            : new SearchQueryResponse<SearchDocument>();


        var response = req.CreateResponse(HttpStatusCode.OK); 
        await response.WriteAsJsonAsync(result, cancellationToken);
        return response;
    }
}