using Azure.Search.Documents.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;
using Azure.Search.Documents;
using IndexHelper.Models;
using Search.CogServices;

namespace CustomBlobIndexer.Functions;

/// <summary>
/// Used search the Cognitive Search index.
/// </summary>
public class IndexSearchFunction
{
    private readonly IndexAppSettings _indexAppSettings;
    private readonly IAcmeCogSearchService _cogSearchDocumentService;
    private readonly ILogger _logger;

    /// <summary>
    /// Constructor
    /// </summary>
    public IndexSearchFunction(ILoggerFactory loggerFactory, IndexAppSettings indexAppSettings, IAcmeCogSearchService cogSearchDocumentService)
    {
        _logger = loggerFactory.CreateLogger<IndexSearchFunction>();
        _indexAppSettings = indexAppSettings;
        _cogSearchDocumentService = cogSearchDocumentService;
    }

    [Function("Index-Search")]
    public async Task<HttpResponseData> IndexSearchAsync([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req, CancellationToken cancellationToken)
    {
        _logger.LogInformation("C# Search request!!!");

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        AcmeSearchQuery? request = JsonConvert.DeserializeObject<AcmeSearchQuery>(requestBody);

        AcmeSearchQueryResult<SearchResult<SearchDocument>> result;

        if (request != null)
        {
            SearchOptions searchOptions;
            switch (request.QueryType)
            {
                case SearchQueryType.Semantic:
                    searchOptions = _cogSearchDocumentService.CreateSemanticSearchOptions(request,
                        _indexAppSettings.CognitiveSearchSemanticConfigurationName);
                    break;
                default:
                    searchOptions = _cogSearchDocumentService.CreateDefaultSearchOptions(request);
                    break;
            }

            // SEARCH!!!
            // SEARCH!!!
            // SEARCH!!!
            result = await _cogSearchDocumentService.SearchAsync(request, searchOptions, null, cancellationToken);
        }
        else
        {
            result = new AcmeSearchQueryResult<SearchResult<SearchDocument>>();
        }

        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(result, cancellationToken);
        return response;
    }
}