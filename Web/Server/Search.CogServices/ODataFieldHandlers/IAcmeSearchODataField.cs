namespace Search.CogServices;

public interface IAcmeSearchODataField
{
    /// <summary>
    /// Indicates if the handler can projects the field type.
    /// </summary>
    /// <param name="fieldType">The type of field.</param>
    bool CanHandle(AcmeSearchFilterFieldTypeEnum fieldType);

    /// <summary>Creates an OData filter string.</summary>
    /// <param name="fieldName">The name of the field in the Azure Index document (it is case sensitive!)</param>
    /// <param name="searchOperator">The operator to use while building the filter.</param>
    /// <param name="values">The values to use while building the filter.</param>
    /// <returns>An OData filer</returns>
    string CreateFilter(string fieldName, AcmeSearchFilterOperatorEnum searchOperator, List<string?> values);
}