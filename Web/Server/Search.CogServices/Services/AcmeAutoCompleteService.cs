using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;

namespace Search.CogServices;

public class AcmeAutoCompleteService : IAcmeAutoCompleteService
{
    private readonly IAcmeODataService _oDataService;
    private readonly IAcmeSearchIndexService _searchIndexService;

    /// <summary>Constructor</summary>
    public AcmeAutoCompleteService(IAcmeSearchIndexService searchIndexService,
        IAcmeODataService oDataService)
    {
        _oDataService = oDataService;
        _searchIndexService = searchIndexService;
    }
    
    /// <summary>Autocomplete</summary>
    /// <param name="request">A request for a suggestion</param>
    /// <param name="securityTrimmingFieldName">The name of the field (as specified in the Azure Index and it is case sensitive)
    /// being used for security trimming.  It's assumed that it is a string collection.</param>
    /// <param name="securityTrimmingValues">The values that the current user has that we will try to match.  In other words, if they have the 'admin' role,
    /// we will only bring back records that have the 'admin' role on them.</param>
    /// <returns>List of suggestions</returns>
    public virtual async Task<Response<AutocompleteResults>> AutoCompleteAsync(AcmeAutoCompleteQuery request,
        string? securityTrimmingFieldName = null, List<string?>? securityTrimmingValues = null)
    {
        var options = CreateDefaultOptions(request, securityTrimmingFieldName, securityTrimmingValues);

        return await AutoCompleteAsync(request, options);
    }

    /// <summary>Autocomplete</summary>
    /// <param name="request">A request for a suggestion</param>
    /// <param name="options">Options that allow specifying autocomplete behaviors, like fuzzy matching.</param>
    /// <returns>List of suggestions</returns>
    public virtual async Task<Response<AutocompleteResults>> AutoCompleteAsync(AcmeAutoCompleteQuery request, AutocompleteOptions options)
    {
        Response<AutocompleteResults> autoCompleteResult = await _searchIndexService.AutocompleteAsync(
            request.IndexName, request.Query, request.SuggestorName, options);

        return autoCompleteResult;
    }


    /// <summary>Creates a set of default options you can then override if necessary.</summary>
    /// <param name="request">The request from the user.</param>
    /// <param name="securityTrimmingFieldName">The name of the field (as specified in the Azure Index and it is case sensitive)
    /// being used for security trimming.  It's assumed that it is a string collection.</param>
    /// <param name="securityTrimmingValues">The values that the current user has that we will try to match.  In other words, if they have the 'admin' role,
    /// we will only bring back records that have the 'admin' role on them.</param>
    public virtual AutocompleteOptions CreateDefaultOptions(AcmeAutoCompleteQuery request,
        string? securityTrimmingFieldName = null, List<string?>? securityTrimmingValues = null)
    {
        string filter = _oDataService.BuildODataFilter(request.IndexName, request.Filters, securityTrimmingFieldName, securityTrimmingValues);

        var options = new AutocompleteOptions
        {
            Filter = filter,
            HighlightPreTag = request.HighlightPreTag,
            HighlightPostTag = request.HighlightPostTag,
            Mode = AutocompleteMode.TwoTerms,
            Size = request.NumberOfSuggestionsToRetrieve,
            UseFuzzyMatching = request.UseFuzzyMatching // the default is false for performance reasons.  My experience shows that it really does not work well with autocomplete, but works fine with Suggest.
        };

        if (request.SearchFields != null)
        {
            foreach (string fieldName in request.SearchFields)
            {
                options.SearchFields.Add(fieldName);
            }
        }
  
       
        return options;
    }
 
}