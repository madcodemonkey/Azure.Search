namespace Search.Model;

public class AcmeSearchFilterForDoubleField : AcmeSearchFilterBase
{
    public AcmeSearchFilterForDoubleField(int id, string fieldName, string displayName, bool isFacetable, bool isSecurityFilter) :
        base(id, displayName, fieldName, isFacetable, isSecurityFilter)
    { }

    protected override string GetFilter(AcmeSearchFilterOperatorEnum searchOperator, params string[] values)
    {
        if (values == null || values.Length == 0)
            return string.Empty;

        return $"{this.FieldName} {OperatorToString(searchOperator)} {values[0]}";
    }
}