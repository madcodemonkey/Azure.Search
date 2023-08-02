namespace Search.CogServices;

public interface IAcmeCogODataService
{
    /// <summary>Builds and OData filter for Azure Search based on user specified filters and the roles that user has been assigned.</summary>
    /// <param name="indexName">The name of the Azure Index</param>
    /// <param name="fieldFilters">A list of field filter where each represents a grouping of filters for one field.</param>
    /// <param name="securityTrimmingFieldName">The name of the field (as specified in the Azure Index and it is case sensitive)
    /// being used for security trimming.  It's assumed that it is a string collection.</param>
    /// <param name="securityTrimmingValues">The values that the current user has that we will try to match.  In other words, if they have the 'admin' role,
    /// we will only bring back records that have the 'admin' role on them.</param>
    /// <returns>An OData Filter</returns>
    string BuildODataFilter(string indexName, List<AcmeSearchFilterField>? fieldFilters,
        string? securityTrimmingFieldName = null, List<string?>? securityTrimmingValues = null);
}