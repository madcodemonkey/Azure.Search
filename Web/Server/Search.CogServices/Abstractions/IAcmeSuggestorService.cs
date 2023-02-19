using Azure.Search.Documents;
using Azure.Search.Documents.Models;

namespace Search.CogServices;

public interface IAcmeSuggestorService
{
    /// <summary>Suggest</summary>
    /// <param name="request">A request for a suggestion</param>
    /// <param name="securityTrimmingFieldName">The name of the field (as specified in the Azure Index and it is case sensitive)
    /// being used for security trimming.  It's assumed that it is a string collection.</param>
    /// <param name="securityTrimmingValues">The values that the current user has that we will try to match.  In other words, if they have the 'admin' role,
    /// we will only bring back records that have the 'admin' role on them.</param>
    /// <returns>List of suggestions</returns>
    Task<SuggestResults<SearchDocument>> SuggestAsync(AcmeSuggestorQuery request,
        string? securityTrimmingFieldName = null, List<string?>? securityTrimmingValues = null);

    /// <summary>Suggest</summary>
    /// <param name="request">A request for a suggestion</param>
    /// <param name="options">The search options to apply</param>
    /// <returns>List of suggestions</returns>
    Task<SuggestResults<SearchDocument>> SuggestAsync(AcmeSuggestorQuery request, SuggestOptions options);

    /// <summary>Creates a set of default options you can then override if necessary.</summary>
    /// <param name="request">The request from the user.</param>
    /// <param name="securityTrimmingFieldName">The name of the field (as specified in the Azure Index and it is case sensitive)
    /// being used for security trimming.  It's assumed that it is a string collection.</param>
    /// <param name="securityTrimmingValues">The values that the current user has that we will try to match.  In other words, if they have the 'admin' role,
    /// we will only bring back records that have the 'admin' role on them.</param>
    SuggestOptions CreateDefaultOptions(AcmeSuggestorQuery request,
        string? securityTrimmingFieldName = null, List<string?>? securityTrimmingValues = null);
}