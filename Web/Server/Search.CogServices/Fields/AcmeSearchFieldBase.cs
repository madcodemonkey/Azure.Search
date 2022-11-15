namespace Search.CogServices;

public abstract class AcmeSearchFieldBase : IAcmeSearchField
{
    /// <summary>Constructor</summary>
    protected AcmeSearchFieldBase(int id, string propertyFieldName, string indexFieldName, string displayName,
        bool isFilterable, bool isSortable, bool isFacetable, bool isHighlighted, bool isSecurityFilter)
    {
        Id = id;
        DisplayName = displayName;
        IndexFieldName = indexFieldName;
        PropertyFieldName = propertyFieldName;
        IsFacetable = isFacetable;
        IsFilterable = isFilterable;
        IsHighlighted = isHighlighted;
        IsSecurityFilter = isSecurityFilter;
        IsSortable = isSortable;
    }

    /// <summary>A unique number identifying for a document.</summary>
    public int Id { get; private set; }

    /// <summary>The name you would display in the UI.</summary>
    public string DisplayName { get; private set; }
    
    /// <summary>The actual field name that we would NEVER send to the user.</summary>
    public string IndexFieldName { get; private set; }

    /// <summary>The name of the property on the C# class that maps to the field.</summary>
    public string PropertyFieldName { get; private set; }

    /// <summary>Indicates if the filter can also be used as a facet.</summary>
    public bool IsFacetable { get; private set; }
    
    /// <summary>Indicates if the field can be used in a filter statement (e.g,. $filter=fieldName eq 'value')</summary>
    public bool IsFilterable { get; private set; }

    /// <summary>Indicates if the field's data is highlighted when doing a search.  Realize that highlights come back in there on collection and
    /// are not part of the document.</summary>
    public bool IsHighlighted { get; private set; }

    /// <summary>Indicates that this filter is used for security trimming</summary>
    public bool IsSecurityFilter { get; private set; }
    
    /// <summary>Indicates if the field can be used in an order by statement (e.g,. $orderby=fieldName desc)</summary>
    public bool IsSortable { get; private set; }

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