using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;

namespace Search.CogServices;

public interface IAcmeAutoCompleteService
{
    /// <summary>Autocomplete</summary>
    /// <param name="request">A request for a suggestion</param>
    /// <param name="securityTrimmingFilter">An optional security trimming filter.</param>
    /// <returns>List of suggestions</returns>
    Task<Response<AutocompleteResults>> AutoCompleteAsync(AcmeAutoCompleteQuery request, IAcmeSecurityTrimmingFilter? securityTrimmingFilter = null);

    /// <summary>Autocomplete</summary>
    /// <param name="request">A request for a suggestion</param>
    /// <param name="options">Options that allow specifying autocomplete behaviors, like fuzzy matching.</param>
    /// <returns>List of suggestions</returns>
    Task<Response<AutocompleteResults>> AutoCompleteAsync(AcmeAutoCompleteQuery request, AutocompleteOptions options);

    /// <summary>Creates a set of default options you can then override if necessary.</summary>
    /// <param name="request">The request from the user.</param>
    /// <param name="securityTrimmingFilter">An optional security trimming filter.</param>
    AutocompleteOptions CreateDefaultOptions(AcmeAutoCompleteQuery request, IAcmeSecurityTrimmingFilter? securityTrimmingFilter = null);
}