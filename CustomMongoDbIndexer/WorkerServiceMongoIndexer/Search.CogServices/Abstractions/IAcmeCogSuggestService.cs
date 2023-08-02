using Azure.Search.Documents;
using Azure.Search.Documents.Models;

namespace Search.CogServices;

public interface IAcmeCogSuggestService
{
    /// <summary>Creates a set of default options you can then override if necessary.</summary>
    /// <param name="request">The request from the user.</param>
    /// <param name="securityTrimmingFieldName">The name of the field (as specified in the Azure Index and it is case sensitive)
    /// being used for security trimming.  It's assumed that it is a string collection.</param>
    /// <param name="securityTrimmingValues">The values that the current user has that we will try to match.  In other words, if they have the 'admin' role,
    /// we will only bring back records that have the 'admin' role on them.</param>
    SuggestOptions CreateDefaultOptions(AcmeSuggestQuery request,
        string? securityTrimmingFieldName = null, List<string?>? securityTrimmingValues = null);

    /// <summary>Suggest</summary>
    /// <param name="request">A request for a suggestion</param>
    /// <param name="securityTrimmingFieldName">The name of the field (as specified in the Azure Index and it is case sensitive)
    /// being used for security trimming.  It's assumed that it is a string collection.</param>
    /// <param name="securityTrimmingValues">The values that the current user has that we will try to match.  In other words, if they have the 'admin' role,
    /// we will only bring back records that have the 'admin' role on them.</param>
    /// <param name="cancellationToken">Optional <see cref="CancellationToken"/> to propagate notifications that the operation should be canceled. </param>
    /// <returns>List of suggestions</returns>
    Task<SuggestResults<SearchDocument>> SuggestAsync(AcmeSuggestQuery request,
        string? securityTrimmingFieldName = null, List<string?>? securityTrimmingValues = null, CancellationToken cancellationToken = default);

    /// <summary>Suggest</summary>
    /// <param name="request">A request for a suggestion</param>
    /// <param name="options">The search options to apply</param>
    /// <param name="securityTrimmingFieldName">The name of the field (as specified in the Azure Index and it is case sensitive)
    /// being used for security trimming.  It's needed here to remove it from the document results.</param>
    /// <param name="cancellationToken">Optional <see cref="CancellationToken"/> to propagate notifications that the operation should be canceled. </param>
    /// <returns>List of suggestions</returns>
    Task<SuggestResults<SearchDocument>> SuggestAsync(AcmeSuggestQuery request, SuggestOptions options, string? securityTrimmingFieldName, CancellationToken cancellationToken = default);

    /// <summary>Used for autocomplete to get a suggestion.</summary>
    /// <typeparam name="T">The type of data being returned.</typeparam>
    /// <param name="indexName">The name of the index</param>
    /// <param name="searchText">The text to find</param>
    /// <param name="suggesterName">The name of the suggestor</param>
    /// <param name="options">The search options to apply</param>
    /// <param name="cancellationToken">Optional <see cref="CancellationToken"/> to propagate notifications that the operation should be canceled. </param>
    Task<SuggestResults<T>> SuggestByIndexNameAsync<T>(string indexName, string searchText, string suggesterName,
        SuggestOptions? options = null, CancellationToken cancellationToken = default);

}