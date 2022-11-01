namespace Search.CogServices;

public class AcmeSearchFilterForNumberField : AcmeSearchFilterBase
{
    /// <summary>Constructor</summary>
    public AcmeSearchFilterForNumberField(int id, string fieldName, string displayName, bool isFacetable, bool isSecurityFilter) :
        base(id, displayName, fieldName, isFacetable, isSecurityFilter)
    { }

    /// <summary>This protected method builds the filter for numeric (double, decimal, float, etc.) types.</summary>
    /// <param name="searchOperator">The operator to use while building the filter.</param>
    /// <param name="values">The values to use while building the filter.</param>
    /// <returns>An OData filer</returns>
    protected override string GetFilter(AcmeSearchFilterOperatorEnum searchOperator, List<string?> values)
    {
        if (values == null || values.Count == 0)
            return string.Empty;

        // TODO: Implement range
        if (searchOperator == AcmeSearchFilterOperatorEnum.WithinRange)
        {
            if (values.Count < 2)
                throw new ArgumentException($"To use the {AcmeSearchFilterOperatorEnum.WithinRange} operator with a number field, you must include at least two values!");

            return $"{this.FieldName} {OperatorToString(AcmeSearchFilterOperatorEnum.GreaterOrEqual)} {ConvertToNullOrTrimString(values[0])} and " +
                $"{this.FieldName} {OperatorToString(AcmeSearchFilterOperatorEnum.LessOrEqual)} {ConvertToNullOrTrimString(values[1])}";
        }

        return $"{this.FieldName} {OperatorToString(searchOperator)} {ConvertToNullOrTrimString(values[0])}";
    }

    protected string ConvertToNullOrTrimString(string? value)
    {
        return value == null ? "null" : value.Trim();
    }
}