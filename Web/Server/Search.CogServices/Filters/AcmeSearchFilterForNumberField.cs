namespace Search.CogServices;

public class AcmeSearchFilterForNumberField : AcmeSearchFilterBase
{
    /// <summary>Constructor</summary>
    public AcmeSearchFilterForNumberField(int id, string fieldName, string displayName, bool isFacetable, bool isSecurityFilter) :
        base(id, displayName, fieldName, isFacetable, isSecurityFilter)
    { }

    protected override string GetFilter(AcmeSearchFilterOperatorEnum searchOperator, params string[] values)
    {
        if (values == null || values.Length == 0)
            return string.Empty;

        // TODO: Implement range
        if (searchOperator == AcmeSearchFilterOperatorEnum.WithinRange)
        {

        }

        return $"{this.FieldName} {OperatorToString(searchOperator)} {values[0]}";
    }
}