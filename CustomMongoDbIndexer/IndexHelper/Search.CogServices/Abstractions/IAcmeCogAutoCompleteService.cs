using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;

namespace Search.CogServices;

/// <summary>
/// This can be called after a user has typed some text and perhaps set a couple of filters. It can handle large quantities of text and bring back one,
/// two or two and some context text back as a suggestion. It is NOT recommended as an as-you-type service. In that case, see <see cref="AcmeCogSuggestService"/>.
/// If you are trying to manipulate the index, get, delete or upload documents <see cref="AcmeCogIndexService"/>.
/// See also these other classes used for searching: <see cref="AcmeCogSearchService"/> or <see cref="AcmeCogSuggestService"/>
/// </summary>
public interface IAcmeCogAutoCompleteService
{
    /// <summary>Autocomplete</summary>
    /// <param name="request">A request for a suggestion</param>
    /// <param name="securityTrimmingFieldName">The name of the field (as specified in the Azure Index and it is case sensitive)
    /// being used for security trimming.  It's assumed that it is a string collection.</param>
    /// <param name="securityTrimmingValues">The values that the current user has that we will try to match.  In other words, if they have the 'admin' role,
    /// we will only bring back records that have the 'admin' role on them.</param>
    /// <param name="cancellationToken">Optional <see cref="CancellationToken"/> to propagate notifications that the operation should be canceled. </param>
    /// <returns>List of suggestions</returns>
    Task<Response<AutocompleteResults>> AutoCompleteAsync(AcmeAutoCompleteQuery request,
        string? securityTrimmingFieldName = null, List<string?>? securityTrimmingValues = null, CancellationToken cancellationToken = default);

    /// <summary>Autocomplete</summary>
    /// <param name="request">A request for a suggestion</param>
    /// <param name="options">Options that allow specifying autocomplete behaviors, like fuzzy matching.</param>
    /// <param name="cancellationToken">Optional <see cref="CancellationToken"/> to propagate notifications that the operation should be canceled. </param>
    /// <returns>List of suggestions</returns>
    Task<Response<AutocompleteResults>> AutoCompleteAsync(AcmeAutoCompleteQuery request, AutocompleteOptions options, CancellationToken cancellationToken = default);

    /// <summary>Retrieves a single document.</summary>
    /// <param name="indexName">The name of the index</param>
    /// <param name="searchText">The partial bit of text to search upon</param>
    /// <param name="suggesterName">The name of the suggester</param>
    /// <param name="options">Options that allow specifying autocomplete behaviors, like fuzzy matching.</param>
    /// <param name="cancellationToken">Optional <see cref="CancellationToken"/> to propagate notifications that the operation should be canceled. </param>
    Task<Response<AutocompleteResults>> AutocompleteByIndexNameAsync(string indexName, string searchText, string suggesterName,
        AutocompleteOptions? options = null, CancellationToken cancellationToken = default);
 
    /// <summary>Creates a set of default options you can then override if necessary.</summary>
    /// <param name="request">The request from the user.</param>
    /// <param name="securityTrimmingFieldName">The name of the field (as specified in the Azure Index and it is case sensitive)
    /// being used for security trimming.  It's assumed that it is a string collection.</param>
    /// <param name="securityTrimmingValues">The values that the current user has that we will try to match.  In other words, if they have the 'admin' role,
    /// we will only bring back records that have the 'admin' role on them.</param>
    AutocompleteOptions CreateDefaultOptions(AcmeAutoCompleteQuery request,
        string? securityTrimmingFieldName = null, List<string?>? securityTrimmingValues = null);
}