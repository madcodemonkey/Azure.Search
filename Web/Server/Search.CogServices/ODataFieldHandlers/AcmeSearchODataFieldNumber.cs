namespace Search.CogServices;

public class AcmeSearchODataFieldNumber : AcmeSearchODataFieldBase
{
    /// <summary>
    /// Indicates if the handler can projects the field type.
    /// </summary>
    /// <param name="fieldType">The type of field.</param>
    public override bool CanHandle(AcmeSearchFilterFieldTypeEnum fieldType) => fieldType == AcmeSearchFilterFieldTypeEnum.Number;

    protected string ConvertToNullOrTrimString(string? value)
    {
        return value == null ? "null" : value.Trim();
    }

    /// <summary>The method that is overriden by the superclass that will be specific to the type specified by the class name.</summary>
    /// <param name="fieldName">The name of the field in the Azure Index document (it is case sensitive!)</param>
    /// <param name="searchOperator">The operator to use while building the filter.</param>
    /// <param name="values">The values to use while building the filter.</param>
    /// <returns>An OData filer</returns>
    protected override string GetFilter(string fieldName, AcmeSearchFilterOperatorEnum searchOperator, List<string?> values)
    {
        if (values == null || values.Count == 0)
            return string.Empty;

        // TODO: Implement range
        if (searchOperator == AcmeSearchFilterOperatorEnum.WithinRange)
        {
            if (values.Count < 2)
                throw new ArgumentException($"To use the {AcmeSearchFilterOperatorEnum.WithinRange} operator with a number field, you must include at least two values!");

            return $"{fieldName} {OperatorToString(AcmeSearchFilterOperatorEnum.GreaterOrEqual)} {ConvertToNullOrTrimString(values[0])} and " +
                $"{fieldName} {OperatorToString(AcmeSearchFilterOperatorEnum.LessOrEqual)} {ConvertToNullOrTrimString(values[1])}";
        }

        return $"{fieldName} {OperatorToString(searchOperator)} {ConvertToNullOrTrimString(values[0])}";
    }
}