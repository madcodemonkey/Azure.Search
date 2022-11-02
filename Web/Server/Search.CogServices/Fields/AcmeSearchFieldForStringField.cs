namespace Search.CogServices;

public class AcmeSearchFieldForStringField : AcmeSearchFieldBase
{
    /// <summary>Constructor</summary>
    public AcmeSearchFieldForStringField(int id, string fieldName, string displayName, bool isFilterable, bool isSortable, bool isFacetable, bool isSecurityFilter) : 
        base(id, displayName, fieldName, isFilterable, isSortable, isFacetable, isSecurityFilter)
    {
    }
   
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