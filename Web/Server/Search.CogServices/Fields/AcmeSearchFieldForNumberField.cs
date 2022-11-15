namespace Search.CogServices;

public class AcmeSearchFieldForNumberField : AcmeSearchFieldBase
{
    /// <summary>Constructor</summary>
    public AcmeSearchFieldForNumberField(int id, string indexFieldName, string displayName, bool isFilterable, bool isSortable, bool isFacetable, bool isHighlighted, bool isSecurityFilter) : 
        base(id, displayName, indexFieldName, isFilterable, isSortable, isFacetable, isHighlighted, isSecurityFilter)
    {
    }
    
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

            return $"{this.IndexFieldName} {OperatorToString(AcmeSearchFilterOperatorEnum.GreaterOrEqual)} {ConvertToNullOrTrimString(values[0])} and " +
                $"{this.IndexFieldName} {OperatorToString(AcmeSearchFilterOperatorEnum.LessOrEqual)} {ConvertToNullOrTrimString(values[1])}";
        }

        return $"{this.IndexFieldName} {OperatorToString(searchOperator)} {ConvertToNullOrTrimString(values[0])}";
    }

    protected string ConvertToNullOrTrimString(string? value)
    {
        return value == null ? "null" : value.Trim();
    }
}