using Azure.Search.Documents;
using Azure.Search.Documents.Models;

namespace Search.CogServices;

public interface IAcmeSearchService
{
    /// <summary>The Search index service, which is a wrapper around Microsoft's SearchIndexClient class.</summary>

    /// <summary>Creates a set of default options you can then override if necessary.</summary>
    /// <param name="request">The request from the user.</param>
    /// <param name="securityTrimmingFilter">An optional security trimming filter.</param>
    SearchOptions CreateDefaultSearchOptions(AcmeSearchQuery request,
        IAcmeSecurityTrimmingFilter? securityTrimmingFilter = null);

    /// <summary>Creates a search options object for semantic search.  Semantic search options can't contain
    /// certain things and need others that are different form a normal search.</summary>
    /// <param name="request">The request from the user.</param>
    /// <param name="configurationName">The name of the semantic configuration in the Azure Portal that should be used.</param>
    /// <param name="securityTrimmingFilter">An optional security trimming filter.</param>
    SearchOptions CreateSemanticSearchOptions(AcmeSearchQuery request, string configurationName,
        IAcmeSecurityTrimmingFilter? securityTrimmingFilter = null);

    /// <summary>Searches using the Azure Search API.  Used for Simple or Full searches only.</summary>
    /// <param name="request">The request from the user.</param>
    /// <param name="securityTrimmingFilter">An optional security trimming filter.</param>
    Task<AcmeSearchQueryResult<SearchResult<SearchDocument>>> SearchAsync(AcmeSearchQuery request,
        IAcmeSecurityTrimmingFilter? securityTrimmingFilter = null);

    /// <summary>Searches using the Azure Search API for the search type of your choice since you are controlling the options.</summary>
    /// <param name="request">The request from the user.</param>
    /// <param name="options">The search options to use when searching for data in Azure Search.</param>
    /// <param name="securityTrimmingFieldName">The name of the field (as specified in the Azure Index and it is case sensitive)
    /// being used for security trimming.  It's needed here to remove it from the document results.</param>
    Task<AcmeSearchQueryResult<SearchResult<SearchDocument>>> SearchAsync(AcmeSearchQuery request, SearchOptions options, string? securityTrimmingFieldName);

    /// <summary>Searches using the Azure Search API for semantic searches only.</summary>
    /// <param name="request">The request from the user.</param>
    /// <param name="configurationName">The name of the semantic configuration in the Azure Portal that should be used.</param>
    /// <param name="securityTrimmingFilter">An optional security trimming filter.</param>
    Task<AcmeSearchQueryResult<SearchResult<SearchDocument>>> SemanticSearchAsync(AcmeSearchQuery request, string configurationName,
        IAcmeSecurityTrimmingFilter? securityTrimmingFilter = null);
}