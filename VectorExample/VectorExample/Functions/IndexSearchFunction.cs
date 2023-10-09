using System.Net;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using CogSimple.Services;
using VectorExample.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace VectorExample.Functions;

public class IndexSearchFunction
{
    private readonly ApplicationSettings _appSettings;
    private readonly ICogSearchIndexService _cogSearchIndexService;
    private readonly IOpenAIService _openAIService;
    private readonly ILogger _logger;

    /// <summary>
    /// Constructor
    /// </summary>
    public IndexSearchFunction(ILoggerFactory loggerFactory,
        IOptions<ApplicationSettings> appSettings,
        ICogSearchIndexService cogSearchIndexService,
        IOpenAIService openAIService)
    {
        _appSettings = appSettings.Value;
        _cogSearchIndexService = cogSearchIndexService;
        _openAIService = openAIService;
        _logger = loggerFactory.CreateLogger<IndexSearchFunction>();
    }

    [Function("IndexSearch")]
    public async Task<HttpResponseData> IndexSearch([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req, CancellationToken cancellationToken)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        SearchQueryResponse<SearchDocument> result;

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        SearchRequest? request = JsonConvert.DeserializeObject<SearchRequest>(requestBody);
        
        if (request != null)
        {
            var searchOptions = new SearchOptions
            {
                IncludeTotalCount = request.IncludeCount,
                QueryType = request.QueryType,
                SearchMode = request.IncludeAllWords ? SearchMode.All : SearchMode.Any
            };

            if (request.QueryType == SearchQueryType.Semantic)
            {
                searchOptions.QueryLanguage = QueryLanguage.EnUs;
                searchOptions.SemanticConfigurationName = _appSettings.CognitiveSearchSemanticConfigurationName;
            }

            if (request.DocumentFields != null)
            {
                foreach (string field in request.DocumentFields)
                {
                    searchOptions.Select.Add(field);
                }
            }

            if (request.SearchFields != null)
            {
                foreach (string field in request.SearchFields)
                {
                    searchOptions.SearchFields.Add(field);
                }
            }

            if (request.VectorFields != null && request.VectorFields.Count > 0 && string.IsNullOrWhiteSpace(request.Query) == false)
            {
                var embedding = await _openAIService.GenerateEmbeddingAsync(request.Query, cancellationToken);
                var searchQueryVector = new SearchQueryVector() { Value = embedding, KNearestNeighborsCount = 3 };
                foreach (string field in request.VectorFields)
                {
                    searchQueryVector.Fields.Add(field);
                }

                searchOptions.Vectors.Add(searchQueryVector);
            }

            string? query = request.VectorOnlySearch ? null : request.Query;

            result = await _cogSearchIndexService.SearchAsync<SearchDocument>(
                _appSettings.CognitiveSearchIndexName, query, searchOptions, cancellationToken);
        }
        else
        {
            result = new SearchQueryResponse<SearchDocument>();
        }
        
        var response = req.CreateResponse(HttpStatusCode.OK); 
        await response.WriteAsJsonAsync(result, cancellationToken);
        return response;
    }
}