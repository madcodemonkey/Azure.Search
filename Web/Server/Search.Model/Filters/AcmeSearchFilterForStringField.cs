namespace Search.Model;

public class AcmeSearchFilterForStringField : AcmeSearchFilterBase
{
    public AcmeSearchFilterForStringField(int id, string fieldName, string displayName, bool isFacetable, bool isSecurityFilter) :
        base(id, displayName, fieldName, isFacetable, isSecurityFilter)
    { }

    protected override string GetFilter(AcmeSearchFilterOperatorEnum searchOperator, params string[] values)
    {
        return $"{this.FieldName} {OperatorToString(searchOperator)} '{values[0]}'";
    }
}