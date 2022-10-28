namespace Search.CogServices;

public class AcmeSearchFilterForBooleanField : AcmeSearchFilterBase
{
    public AcmeSearchFilterForBooleanField(int id, string fieldName, string displayName, bool isFacetable, bool isSecurityFilter) :
        base(id, displayName, fieldName, isFacetable, isSecurityFilter)
    { }

    protected override string GetFilter(AcmeSearchFilterOperatorEnum searchOperator, params string[] values)
    {
        if (values == null || values.Length == 0)
            return string.Empty;

        if (searchOperator != AcmeSearchFilterOperatorEnum.Equal && searchOperator != AcmeSearchFilterOperatorEnum.NotEqual)
            throw new ArgumentException($"Please specify either equal or not equal for boolean field named {FieldName}!");

        string booleanAsString = values[0].ToLower().Trim();
        if (booleanAsString != "true" && booleanAsString != "false")
            throw new ArgumentException($"Please specify either 'true' or 'false' for the field value for the field named {FieldName}!");

        return $"{this.FieldName} {OperatorToString(searchOperator)} {booleanAsString}";
    }
}