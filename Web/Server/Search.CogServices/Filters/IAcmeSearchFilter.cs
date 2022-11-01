namespace Search.CogServices;

public interface IAcmeSearchFilter
{
    /// <summary>Primary identifier for a filter.</summary>
    int Id { get; }

    /// <summary>The name you would display in the UI.</summary>
    string DisplayName { get; }

    /// <summary>The actual field name that we would NEVER send to the user.</summary>
    string FieldName { get; }


    /// <summary>Indicates if the filter can also be used as a facet.</summary>
    bool IsFacetable { get; }

    /// <summary>Indicates that this filter is used for security trimming</summary>
    public bool IsSecurityFilter { get; }

    /// <summary>Values for the filter.</summary>
    /// <param name="searchOperator">The operator (eq, ne, gt, lt, ge, le)</param>
    /// <param name="values">string values for the filter.  Remember that this is an OData filter and it IS CASE SENSTIVE!</param>
    /// <returns>OData filter string</returns>
    string CreateFilter(AcmeSearchFilterOperatorEnum searchOperator, List<string?> values);
}