namespace Search.CogServices;

public class AcmeSearchFilterForStringField : AcmeSearchFilterBase
{
    public AcmeSearchFilterForStringField(int id, string fieldName, string displayName, bool isFacetable, bool isSecurityFilter) :
        base(id, displayName, fieldName, isFacetable, isSecurityFilter)
    { }

    protected override string GetFilter(AcmeSearchFilterOperatorEnum searchOperator, List<string> values)
    {
        // TODO: Null is allowed for values but not taken into account here!

        return $"{this.FieldName} {OperatorToString(searchOperator)} '{values[0]}'";
    }
}