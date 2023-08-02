using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;

namespace Search.CogServices;

public interface IAcmeCogSearchService
{
    /// <summary>The Search index service, which is a wrapper around Microsoft's SearchIndexClient class.</summary>

    /// <summary>Creates a set of default options you can then override if necessary.</summary>
    /// <param name="request">The request from the user.</param>
    /// <param name="securityTrimmingFieldName">The name of the field (as specified in the Azure Index and it is case sensitive)
    /// being used for security trimming.  It's assumed that it is a string collection.</param>
    /// <param name="securityTrimmingValues">The values that the current user has that we will try to match.  In other words, if they have the 'admin' role,
    /// we will only bring back records that have the 'admin' role on them.</param>
    SearchOptions CreateDefaultSearchOptions(AcmeSearchQuery request,
        string? securityTrimmingFieldName = null, List<string?>? securityTrimmingValues = null);

    /// <summary>Creates a search options object for semantic search.  Semantic search options can't contain
    /// certain things and need others that are different form a normal search.</summary>
    /// <param name="request">The request from the user.</param>
    /// <param name="configurationName">The name of the semantic configuration in the Azure Portal that should be used.</param>
    /// <param name="securityTrimmingFieldName">The name of the field (as specified in the Azure Index and it is case sensitive)
    /// being used for security trimming.  It's assumed that it is a string collection.</param>
    /// <param name="securityTrimmingValues">The values that the current user has that we will try to match.  In other words, if they have the 'admin' role,
    /// we will only bring back records that have the 'admin' role on them.</param>
    SearchOptions CreateSemanticSearchOptions(AcmeSearchQuery request, string configurationName,
        string? securityTrimmingFieldName = null, List<string?>? securityTrimmingValues = null);

    /// <summary>Searches using the Azure Search API.  Used for Simple or Full searches only.</summary>
    /// <param name="request">The request from the user.</param>
    /// <param name="securityTrimmingFieldName">The name of the field (as specified in the Azure Index and it is case sensitive)
    /// being used for security trimming.  It's assumed that it is a string collection.</param>
    /// <param name="securityTrimmingValues">The values that the current user has that we will try to match.  In other words, if they have the 'admin' role,
    /// we will only bring back records that have the 'admin' role on them.</param>
    /// <param name="cancellationToken">Optional <see cref="CancellationToken"/> to propagate notifications that the operation should be canceled. </param>
    Task<AcmeSearchQueryResult<SearchResult<SearchDocument>>> SearchAsync(AcmeSearchQuery request,
        string? securityTrimmingFieldName = null, List<string?>? securityTrimmingValues = null, CancellationToken cancellationToken = default);

    /// <summary>Performs a search against the index.</summary>
    /// <typeparam name="T">The type of data being returned.</typeparam>
    /// <param name="indexName">The name of the index</param>
    /// <param name="searchText">The text to find</param>
    /// <param name="options">The search options to apply</param>
    /// <param name="cancellationToken">Optional <see cref="CancellationToken"/> to propagate notifications that the operation should be canceled. </param>
    Task<Response<SearchResults<T>>> SearchAsync<T>(string indexName, string searchText,
        SearchOptions? options = null, CancellationToken cancellationToken = default);

    /// <summary>Searches using the Azure Search API for the search type of your choice since you are controlling the options.</summary>
    /// <param name="request">The request from the user.</param>
    /// <param name="options">The search options to use when searching for data in Azure Search.</param>
    /// <param name="securityTrimmingFieldName">The name of the field (as specified in the Azure Index and it is case sensitive)
    /// being used for security trimming.  It's needed here to remove it from the document results.</param>
    /// <param name="cancellationToken">Optional <see cref="CancellationToken"/> to propagate notifications that the operation should be canceled. </param>
    Task<AcmeSearchQueryResult<SearchResult<SearchDocument>>> SearchAsync(AcmeSearchQuery request, SearchOptions options,
        string? securityTrimmingFieldName, CancellationToken cancellationToken = default);

    /// <summary>Searches using the Azure Search API for semantic searches only.</summary>
    /// <param name="request">The request from the user.</param>
    /// <param name="configurationName">The name of the semantic configuration in the Azure Portal that should be used.</param>
    /// <param name="securityTrimmingFieldName">The name of the field (as specified in the Azure Index and it is case sensitive)
    /// being used for security trimming.  It's assumed that it is a string collection.</param>
    /// <param name="securityTrimmingValues">The values that the current user has that we will try to match.  In other words, if they have the 'admin' role,
    /// we will only bring back records that have the 'admin' role on them.</param>
    Task<AcmeSearchQueryResult<SearchResult<SearchDocument>>> SemanticSearchAsync(AcmeSearchQuery request, string configurationName,
        string? securityTrimmingFieldName = null, List<string?>? securityTrimmingValues = null);
}