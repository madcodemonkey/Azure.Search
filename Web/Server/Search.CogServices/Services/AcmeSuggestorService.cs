using Azure.Search.Documents;
using Azure.Search.Documents.Models;

namespace Search.CogServices;

public class AcmeSuggestorService : IAcmeSuggestorService
{
    private readonly IAcmeODataService _oDataService;
    private readonly IAcmeSearchIndexService _searchIndexService;

    /// <summary>Constructor</summary>
    public AcmeSuggestorService(IAcmeSearchIndexService searchIndexService,
        IAcmeODataService oDataService)
    {
        _oDataService = oDataService;
        _searchIndexService = searchIndexService;
    }


    /// <summary>Suggest</summary>
    /// <param name="request">A request for a suggestion</param>
    /// <param name="securityTrimmingFieldName">The name of the field (as specified in the Azure Index and it is case sensitive)
    /// being used for security trimming.  It's assumed that it is a string collection.</param>
    /// <param name="securityTrimmingValues">The values that the current user has that we will try to match.  In other words, if they have the 'admin' role,
    /// we will only bring back records that have the 'admin' role on them.</param>
    /// <returns>List of suggestions</returns>
    public virtual async Task<SuggestResults<SearchDocument>> SuggestAsync(AcmeSuggestorQueryV2 request,
        string? securityTrimmingFieldName = null, List<string?>? securityTrimmingValues = null)
    {
        var options = CreateDefaultOptions(request, securityTrimmingFieldName, securityTrimmingValues);

        return await SuggestAsync(request, options);
    }

    /// <summary>Suggest</summary>
    /// <param name="request">A request for a suggestion</param>
    /// <param name="options">The search options to apply</param>
    /// <returns>List of suggestions</returns>
    public virtual async Task<SuggestResults<SearchDocument>> SuggestAsync(AcmeSuggestorQueryV2 request, SuggestOptions options)
    {
        var suggestResult = await _searchIndexService.SuggestAsync<SearchDocument>(request.IndexName, request.Query, request.SuggestorName, options);

        return suggestResult;
    }
    
    /// <summary>Creates a set of default options you can then override if necessary.</summary>
    /// <param name="request">The request from the user.</param>
    /// <param name="securityTrimmingFieldName">The name of the field (as specified in the Azure Index and it is case sensitive)
    /// being used for security trimming.  It's assumed that it is a string collection.</param>
    /// <param name="securityTrimmingValues">The values that the current user has that we will try to match.  In other words, if they have the 'admin' role,
    /// we will only bring back records that have the 'admin' role on them.</param>
    public virtual SuggestOptions CreateDefaultOptions(AcmeSuggestorQueryV2 request,
        string? securityTrimmingFieldName = null, List<string?>? securityTrimmingValues = null)
    {
        string filter = _oDataService.BuildODataFilter(request.IndexName, request.Filters, securityTrimmingFieldName, securityTrimmingValues);

        var options = new SuggestOptions
        {
            Filter = filter,
            HighlightPreTag = request.HighlightPreTag,
            HighlightPostTag = request.HighlightPostTag,
            // MinimumCoverage = 33.3,
            OrderBy = { "search.score() desc" },
            Size = request.NumberOfSuggestionsToRetrieve,
            UseFuzzyMatching = request.UseFuzzyMatching // false by default for performance reasons
        };

        if (request.SearchFields != null)
        {
            foreach (string fieldName in request.SearchFields)
            {
                options.SearchFields.Add(fieldName);
            }
        }

        if (request.SelectFields != null)
        {
            foreach (string fieldName in request.SelectFields)
            {
                options.Select.Add(fieldName);
            }
        }

        if (request.DocumentFields != null)
        {
            foreach (string fieldName in request.DocumentFields)
            {
                options.Select.Add(fieldName);
            }
        }
        

        return options;
    }
}