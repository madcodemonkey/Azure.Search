using Azure.Search.Documents;
using Azure.Search.Documents.Models;

namespace Search.CogServices;

public interface IAcmeSearchService
{
    /// <summary>Searches using the Azure Search API.</summary>
    /// <param name="request">The request from the user.</param>
    /// <param name="securityTrimmingFieldName">The name of the field (as specified in the Azure Index and it is case sensitive)
    /// being used for security trimming.  It's assumed that it is a string collection.</param>
    /// <param name="securityTrimmingValues">The values that the current user has that we will try to match.  In other words, if they have the 'admin' role,
    /// we will only bring back records that have the 'admin' role on them.</param>
    Task<AcmeSearchQueryResultV2<SearchResult<SearchDocument>>> SearchAsync(AcmeSearchQueryV2 request,
        string? securityTrimmingFieldName = null, List<string?>? securityTrimmingValues = null);

    /// <summary>Searches using the Azure Search API.</summary>
    /// <param name="request">The request from the user.</param>
    /// <param name="options">The search options to use when searching for data in Azure Search.</param>
    Task<AcmeSearchQueryResultV2<SearchResult<SearchDocument>>> SearchAsync(AcmeSearchQueryV2 request, SearchOptions options);

    /// <summary>Searches using the Azure Search API.</summary>
    /// <param name="request">The request from the user.</param>
    /// <param name="configurationName">The name of the semantic configuration in the Azure Portal that should be used.</param>
    /// <param name="securityTrimmingFieldName">The name of the field (as specified in the Azure Index and it is case sensitive)
    /// being used for security trimming.  It's assumed that it is a string collection.</param>
    /// <param name="securityTrimmingValues">The values that the current user has that we will try to match.  In other words, if they have the 'admin' role,
    /// we will only bring back records that have the 'admin' role on them.</param>
    Task<AcmeSearchQueryResultV2<SearchResult<SearchDocument>>> SemanticSearchAsync(AcmeSearchQueryV2 request, string configurationName,
        string? securityTrimmingFieldName = null, List<string?>? securityTrimmingValues = null);
}