using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using Search.CogServices.Extensions;

namespace Search.CogServices;

/// <summary>
/// Suggest – This is a as-you-type service that makes suggestions. It is NOT recommended for large bodies of text like documents that have been scanned
/// in from a file. In those cases, see <see cref="AcmeCogAutoCompleteService"/>.  If you are trying to manipulate the index, get, delete or upload
/// documents <see cref="AcmeCogIndexService"/>.  See also these other classes used for searching: <see cref="AcmeCogSearchService"/> or <see cref="AcmeCogAutoCompleteService"/>
/// </summary>
public class AcmeCogSuggestService : IAcmeCogSuggestService
{
    private readonly IAcmeCogClientService _clientService;
    private readonly IAcmeCogODataService _cogODataService;

    /// <summary>Constructor</summary>
    public AcmeCogSuggestService(IAcmeCogClientService clientService,
        IAcmeCogODataService cogODataService)
    {
        _clientService = clientService;
        _cogODataService = cogODataService;
    }

    /// <summary>Creates a set of default options you can then override if necessary.</summary>
    /// <param name="request">The request from the user.</param>
    /// <param name="securityTrimmingFieldName">The name of the field (as specified in the Azure Index and it is case sensitive)
    /// being used for security trimming.  It's assumed that it is a string collection.</param>
    /// <param name="securityTrimmingValues">The values that the current user has that we will try to match.  In other words, if they have the 'admin' role,
    /// we will only bring back records that have the 'admin' role on them.</param>
    public virtual SuggestOptions CreateDefaultOptions(AcmeSuggestQuery request,
        string? securityTrimmingFieldName = null, List<string?>? securityTrimmingValues = null)
    {
        string filter = _cogODataService.BuildODataFilter(request.IndexName, request.Filters, securityTrimmingFieldName, securityTrimmingValues);

        var options = new SuggestOptions
        {
            Filter = filter,
            HighlightPreTag = request.HighlightPreTag,
            HighlightPostTag = request.HighlightPostTag,
            // MinimumCoverage = 33.3,
            Size = request.NumberOfSuggestionsToRetrieve,
            UseFuzzyMatching = request.UseFuzzyMatching // false by default for performance reasons
        };

        if (request.DocumentFields != null)
        {
            foreach (string fieldName in request.DocumentFields)
            {
                options.Select.Add(fieldName);
            }
        }

        if (request.OrderByFields != null && request.OrderByFields.Count > 0)
        {
            foreach (AcmeSearchOrderBy orderBy in request.OrderByFields)
            {
                string sortOrder = orderBy.SortDescending ? "desc" : "asc";
                options.OrderBy.Add($"{orderBy.FieldName} {sortOrder}");
            }
        }
        else
        {
            options.OrderBy.Add("search.score() desc");
        }

        if (request.SearchFields != null)
        {
            foreach (string fieldName in request.SearchFields)
            {
                options.SearchFields.Add(fieldName);
            }
        }

        return options;
    }

    /// <summary>Suggest</summary>
    /// <param name="request">A request for a suggestion</param>
    /// <param name="securityTrimmingFieldName">The name of the field (as specified in the Azure Index and it is case sensitive)
    /// being used for security trimming.  It's assumed that it is a string collection.</param>
    /// <param name="securityTrimmingValues">The values that the current user has that we will try to match.  In other words, if they have the 'admin' role,
    /// we will only bring back records that have the 'admin' role on them.</param>
    /// <param name="cancellationToken">Optional <see cref="CancellationToken"/> to propagate notifications that the operation should be canceled. </param>
    /// <returns>List of suggestions</returns>
    public virtual async Task<SuggestResults<SearchDocument>> SuggestAsync(AcmeSuggestQuery request,
        string? securityTrimmingFieldName = null, List<string?>? securityTrimmingValues = null, CancellationToken cancellationToken = default)
    {
        var options = CreateDefaultOptions(request, securityTrimmingFieldName, securityTrimmingValues);

        return await SuggestAsync(request, options, securityTrimmingFieldName, cancellationToken);
    }

    /// <summary>Suggest</summary>
    /// <param name="request">A request for a suggestion</param>
    /// <param name="options">The search options to apply</param>
    /// <param name="securityTrimmingFieldName">The name of the field (as specified in the Azure Index and it is case sensitive)
    /// being used for security trimming.  It's needed here to remove it from the document results.</param>
    /// <param name="cancellationToken">Optional <see cref="CancellationToken"/> to propagate notifications that the operation should be canceled. </param>
    /// <returns>List of suggestions</returns>
    public virtual async Task<SuggestResults<SearchDocument>> SuggestAsync(AcmeSuggestQuery request, SuggestOptions options,
        string? securityTrimmingFieldName, CancellationToken cancellationToken = default)
    {
        var suggestResult = await SuggestByIndexNameAsync<SearchDocument>(request.IndexName, request.Query, request.SuggestorName, options, cancellationToken);

        if (securityTrimmingFieldName != null)
        {
            foreach (SearchSuggestion<SearchDocument> item in suggestResult.Results)
            {
                item.Document.RemoveField(securityTrimmingFieldName);
                item.Document.ReMapFields(request.DocumentFieldMaps);
            }
        }

        return suggestResult;
    }

    /// <summary>Used for autocomplete to get a suggestion.</summary>
    /// <typeparam name="T">The type of data being returned.</typeparam>
    /// <param name="indexName">The name of the index</param>
    /// <param name="searchText">The text to find</param>
    /// <param name="suggesterName">The name of the suggestor</param>
    /// <param name="options">The search options to apply</param>
    /// <param name="cancellationToken">Optional <see cref="CancellationToken"/> to propagate notifications that the operation should be canceled. </param>
    public async Task<SuggestResults<T>> SuggestByIndexNameAsync<T>(string indexName, string searchText, string suggesterName,
        SuggestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var searchClient = _clientService.GetSearchClient(indexName);
        return await searchClient.SuggestAsync<T>(searchText, suggesterName, options, cancellationToken);
    }
}