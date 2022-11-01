namespace Search.CogServices;

public abstract class AcmeSearchFilterBase : IAcmeSearchFilter
{
    /// <summary>Constructor</summary>
    protected AcmeSearchFilterBase(int id, string displayName, string fieldName, bool isFacetable, bool isSecurityFilter)
    {
        Id = id;
        DisplayName = displayName;
        FieldName = fieldName;
        IsFacetable = isFacetable;
        IsSecurityFilter = isSecurityFilter;
    }
    /// <summary>A unique number identifying this facet.</summary>
    public int Id { get; private set; }
    public string DisplayName { get; private set; }
    public string FieldName { get; private set; }
    public bool IsFacetable { get; private set; }
    public bool IsSecurityFilter { get; private set; }

    /// <summary>Creates an OData filter string.</summary>
    /// <param name="searchOperator">The operator to use while building the filter.</param>
    /// <param name="values">The values to use while building the filter.</param>
    /// <returns>An OData filer</returns>
    public string CreateFilter(AcmeSearchFilterOperatorEnum searchOperator, List<string?> values)
    {
        if (values == null || values.Count == 0)
            throw new ArgumentException("You must specify one or more values for a filter!");
        ValidateFilters(values);
        return GetFilter(searchOperator, values);
    }

    /// <summary>The method that is overriden by the superclass that will be specific to the type specified by the class name.</summary>
    /// <param name="searchOperator">The operator to use while building the filter.</param>
    /// <param name="values">The values to use while building the filter.</param>
    /// <returns>An OData filer</returns>
    protected abstract string GetFilter(AcmeSearchFilterOperatorEnum searchOperator, List<string?> values);

    protected string OperatorToString(AcmeSearchFilterOperatorEnum searchOperator)
    {
        // Reference https://docs.microsoft.com/bs-latn-ba/azure/search/search-query-odata-comparison-operators
        switch (searchOperator)
        {
            case AcmeSearchFilterOperatorEnum.Equal:
                return "eq";
            case AcmeSearchFilterOperatorEnum.NotEqual:
                return "ne";
            case AcmeSearchFilterOperatorEnum.GreaterThan:
                return "gt";
            case AcmeSearchFilterOperatorEnum.LessThan:
                return "lt";
            case AcmeSearchFilterOperatorEnum.GreaterOrEqual:
                return "ge";
            case AcmeSearchFilterOperatorEnum.LessOrEqual:
                return "le";
            default:
                return "eq";
        }
    }

    private void ValidateFilters(List<string?> values)
    {
        for (int i = 0; i < values.Count; i++)
        {
            if (values[i] == null) continue;

            if (values[i]?.IndexOf('"') != -1)
                throw new UnauthorizedAccessException("A filter contains in illegal character (double quote)");
            if (values[i]?.IndexOf("'") != -1)
                throw new UnauthorizedAccessException("A filter contains in illegal character (single quote)");
        }
    }

}