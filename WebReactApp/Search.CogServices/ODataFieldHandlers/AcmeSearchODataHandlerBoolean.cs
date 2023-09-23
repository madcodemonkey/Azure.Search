namespace Search.CogServices;

public class AcmeSearchODataHandlerBoolean : AcmeSearchODataHandlerBase
{
    /// <summary>
    /// Indicates if the handler can projects the field type.
    /// </summary>
    /// <param name="fieldType">The type of field.</param>
    public override bool CanHandle(AcmeSearchFilterFieldTypeEnum fieldType) => fieldType == AcmeSearchFilterFieldTypeEnum.Boolean;

    /// <summary>The method that is overriden by the superclass that will be specific to the type specified by the class name.</summary>
    /// <param name="fieldName">The name of the field in the Azure Index document (it is case sensitive!)</param>
    /// <param name="searchOperator">The operator to use while building the filter.</param>
    /// <param name="values">The values to use while building the filter.</param>
    /// <returns>An OData filer</returns>
    protected override string GetFilter(string fieldName, AcmeSearchFilterOperatorEnum searchOperator, List<string?> values)
    {
        if (values == null || values.Count == 0)
            return string.Empty;

        if (searchOperator != AcmeSearchFilterOperatorEnum.Equal && searchOperator != AcmeSearchFilterOperatorEnum.NotEqual)
            throw new ArgumentException($"Please specify either equal or not equal for boolean field named {fieldName}!");
        
        var firstString = values[0];
        string booleanAsString = string.IsNullOrWhiteSpace(firstString) ? "null" : firstString.ToLower().Trim();
        if (booleanAsString != "null" && booleanAsString != "true" && booleanAsString != "false")
            throw new ArgumentException($"Please specify either 'true' or 'false' for the field value for the field named {fieldName}!");

        return $"{fieldName} {OperatorToString(searchOperator)} {booleanAsString}";
    }
}