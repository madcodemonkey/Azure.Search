namespace Search.CogServices;

public interface IAcmeODataService
{
    /// <summary>Builds and OData filter for Azure Search based on user specified filters and the roles that user has been assigned.</summary>
    /// <param name="indexName">The name of the Azure Index</param>
    /// <param name="fieldFilters">A list of field filter where each represents a grouping of filters for one field.</param>
    /// <param name="securityTrimmingValues">Roles assigned to the current user.</param>
    /// <returns>An OData Filter</returns>
    string BuildODataFilter(string indexName, List<AcmeSearchFilterFieldV2> fieldFilters,
        string? securityTrimmingFieldName = null, List<string?>? securityTrimmingValues = null);
}