using Azure.Search.Documents;
using Azure.Search.Documents.Models;

namespace Search.CogServices;

public interface IAcmeSuggestService
{
    /// <summary>Creates a set of default options you can then override if necessary.</summary>
    /// <param name="request">The request from the user.</param>
    /// <param name="securityTrimmingFilter">An optional security trimming filter.</param>
    SuggestOptions CreateDefaultOptions(AcmeSuggestQuery request, IAcmeSecurityTrimmingFilter? securityTrimmingFilter = null);

    /// <summary>Suggest</summary>
    /// <param name="request">A request for a suggestion</param>
    /// <param name="securityTrimmingFilter">An optional security trimming filter.</param>
    /// <returns>List of suggestions</returns>
    Task<SuggestResults<SearchDocument>> SuggestAsync(AcmeSuggestQuery request,
        IAcmeSecurityTrimmingFilter? securityTrimmingFilter = null);

    /// <summary>Suggest</summary>
    /// <param name="request">A request for a suggestion</param>
    /// <param name="options">The search options to apply</param>
    /// <param name="securityTrimmingFieldName">The name of the field (as specified in the Azure Index and it is case sensitive)
    /// being used for security trimming.  It's needed here to remove it from the document results.</param>
    /// <returns>List of suggestions</returns>
    Task<SuggestResults<SearchDocument>> SuggestAsync(AcmeSuggestQuery request, SuggestOptions options, string? securityTrimmingFieldName);
}