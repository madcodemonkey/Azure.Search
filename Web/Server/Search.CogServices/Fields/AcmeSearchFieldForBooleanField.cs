namespace Search.CogServices;

public class AcmeSearchFieldForBooleanField : AcmeSearchFieldBase
{
    /// <summary>Constructor</summary>
    public AcmeSearchFieldForBooleanField(int id, string indexFieldName, string displayName, bool isFilterable, bool isSortable, bool isFacetable, bool isHighlighted, bool isSecurityFilter) :
        base(id, displayName, indexFieldName, isFilterable, isSortable, isFacetable, isHighlighted, isSecurityFilter)
    {
    }

    /// <summary>This protected method builds the filter for the boolean type.</summary>
    /// <param name="searchOperator">The operator to use while building the filter.</param>
    /// <param name="values">The values to use while building the filter.</param>
    /// <returns>An OData filer</returns>
    protected override string GetFilter(AcmeSearchFilterOperatorEnum searchOperator, List<string?> values)
    {
        if (values == null || values.Count == 0)
            return string.Empty;

        if (searchOperator != AcmeSearchFilterOperatorEnum.Equal && searchOperator != AcmeSearchFilterOperatorEnum.NotEqual)
            throw new ArgumentException($"Please specify either equal or not equal for boolean field named {IndexFieldName}!");

        string booleanAsString = values[0]?.ToLower().Trim() ?? "null";
        if (booleanAsString != "null" && booleanAsString != "true" && booleanAsString != "false")
            throw new ArgumentException($"Please specify either 'true' or 'false' for the field value for the field named {IndexFieldName}!");

        return $"{this.IndexFieldName} {OperatorToString(searchOperator)} {booleanAsString}";
    }
}