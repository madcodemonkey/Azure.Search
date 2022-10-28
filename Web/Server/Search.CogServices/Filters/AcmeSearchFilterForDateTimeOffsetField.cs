namespace Search.CogServices;

public class AcmeSearchFilterForDateTimeOffsetField : AcmeSearchFilterBase
{
    /// <summary>Constructor</summary>
    public AcmeSearchFilterForDateTimeOffsetField(int id, string fieldName, string displayName, bool isFacetable, bool isSecurityFilter) :
        base(id, displayName, fieldName, isFacetable, isSecurityFilter)
    { }

    protected override string GetFilter(AcmeSearchFilterOperatorEnum searchOperator, List<string> values)
    {
        if (values == null || values.Count == 0)
            return string.Empty;

        // TODO: Implement range
        if (searchOperator == AcmeSearchFilterOperatorEnum.WithinRange)
        {

        }

        return $"{this.FieldName} {OperatorToString(searchOperator)} {values[0]}";
    }
}