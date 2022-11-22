namespace Search.CogServices;

public class AcmeSearchFieldForChildObject : AcmeSearchFieldBase
{
    private readonly string _parentFieldName;
    private readonly string _childFieldName;

    /// <summary>Constructor</summary>
    public AcmeSearchFieldForChildObject(int id, string propertyFieldName, string indexFieldName, string displayName,
        bool isFilterable, bool isSortable, bool isFacetable, bool isHighlighted, bool isSecurityFilter) : 
        base(id, propertyFieldName, indexFieldName, displayName, isFilterable, isSortable, isFacetable, isHighlighted, isSecurityFilter)
    {
        string[] fields = indexFieldName.Split('/');
        if (fields == null || fields.Length != 2)
            throw new ArgumentException("Please use slash syntax to denote parent and child fields (e.g., 'authors/name' where authors is parent and name is child object field name)");
        _parentFieldName = fields[0];
        _childFieldName = fields[1];
    }

    /// <summary>This protected method builds the filter for the child object type.</summary>
    /// <param name="searchOperator">The operator to use while building the filter.</param>
    /// <param name="values">The values to use while building the filter.</param>
    /// <returns>An OData filer</returns>
    protected override string GetFilter(AcmeSearchFilterOperatorEnum searchOperator, List<string?> values)
    {
        if (values == null || values.Count == 0)
            return string.Empty;

        if (searchOperator != AcmeSearchFilterOperatorEnum.Equal)
            throw new ArgumentException($"Currently we only handle equal operator for collections!  Please correct the search operator for the field named {IndexFieldName}");

        //  return $"authors/any(c: c/name eq '{values[0]}')";
        return $"{this._parentFieldName}/any(c: c/{_childFieldName} eq '{values[0]}')";
    }

}