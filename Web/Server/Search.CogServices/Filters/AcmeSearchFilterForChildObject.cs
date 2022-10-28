namespace Search.CogServices;

public class AcmeSearchFilterForChildObject : AcmeSearchFilterBase
{
    private readonly string _parentFieldName;
    private readonly string _childFieldName;

    public AcmeSearchFilterForChildObject(int id, string displayName, string fieldName, bool isFacetable, bool isSecurityFilter)
        : base(id, displayName, fieldName, isFacetable, isSecurityFilter)
    {
        string[] fields = fieldName.Split('/');
        if (fields == null || fields.Length != 2)
            throw new ArgumentException("Please use slash syntax to denote parent and child fields (e.g., 'authors/name' where authors is parent and name is child object field name)");
        _parentFieldName = fields[0];
        _childFieldName = fields[1];
    }

    protected override string GetFilter(AcmeSearchFilterOperatorEnum searchOperator, params string[] values)
    {
        if (values == null || values.Length == 0)
            return string.Empty;

        if (searchOperator != AcmeSearchFilterOperatorEnum.Equal)
            throw new ArgumentException($"Currently we only handle equal operator for collections!  Please correct the search operator for the field named {FieldName}");

        //  return $"authors/any(c: c/name eq '{values[0]}')";
        return $"{this._parentFieldName}/any(c: c/{_childFieldName} eq '{values[0]}')";
    }

}