namespace Search.CogServices;

public class AcmeSearchFilterForStringField : AcmeSearchFilterBase
{
    /// <summary>Constructor</summary>
    public AcmeSearchFilterForStringField(int id, string fieldName, string displayName, bool isFacetable, bool isSecurityFilter) :
        base(id, displayName, fieldName, isFacetable, isSecurityFilter)
    { }

    /// <summary>This protected method builds the filter for the string type.</summary>
    /// <param name="searchOperator">The operator to use while building the filter.</param>
    /// <param name="values">The values to use while building the filter.</param>
    /// <returns>An OData filer</returns>
    protected override string GetFilter(AcmeSearchFilterOperatorEnum searchOperator, List<string?> values)
    {
        string theValue = values[0] == null ? "null" : $"'{values[0]}'";

        return $"{this.FieldName} {OperatorToString(searchOperator)} {theValue}";
    }
}