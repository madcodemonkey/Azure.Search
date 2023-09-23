namespace Search.CogServices;

public interface IAcmeODataService
{
    /// <summary>Builds and OData filter for Azure Search based on user specified filters and the roles that user has been assigned.</summary>
    /// <param name="indexName">The name of the Azure Index</param>
    /// <param name="fieldFilters">A list of field filter where each represents a grouping of filters for one field.</param>
    /// <param name="securityTrimmingFilter">An optional security trimming filter.</param>
    /// <returns>An OData Filter</returns>
    string? BuildODataFilter(string indexName, List<AcmeSearchFilterField>? fieldFilters, IAcmeSecurityTrimmingFilter? securityTrimmingFilter = null);

    /// <summary>
    /// Finds an instance of an OData hanlder.
    /// </summary>
    /// <param name="fieldType">The field type.</param>
    IAcmeSearchODataHandler? FindHandler(AcmeSearchFilterFieldTypeEnum fieldType);
}