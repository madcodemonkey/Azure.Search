namespace Search.CogServices;

public interface IAcmeSearchField
{
    /// <summary>A unique number identifying for a document.</summary>
    int Id { get; }

    /// <summary>The name you would display in the UI.</summary>
    string DisplayName { get; }

    /// <summary>The actual field name in the Azure Index that we would NEVER send to the user.</summary>
    string IndexFieldName { get; }

    /// <summary>Indicates if the filter can also be used as a facet.</summary>
    bool IsFacetable { get; }

    /// <summary>Indicates if the field can be used in a filter statement (e.g,. $filter=fieldName eq 'value')</summary>
    bool IsFilterable { get; }

    /// <summary>Indicates if the field's data is highlighted when doing a search.  Realize that highlights come back in there on collection and
    /// are not part of the document.</summary>
    public bool IsHighlighted { get; }
    
    /// <summary>Indicates that this filter is used for security trimming</summary>
    public bool IsSecurityFilter { get; }
    
    /// <summary>Indicates if the field can be used in an order by statement (e.g,. $orderby=fieldName desc)</summary>
    public bool IsSortable { get; }
    
    /// <summary>Values for the filter.</summary>
    /// <param name="searchOperator">The operator (eq, ne, gt, lt, ge, le)</param>
    /// <param name="values">string values for the filter.  Remember that this is an OData filter and it IS CASE SENSTIVE!</param>
    /// <returns>OData filter string</returns>
    string CreateFilter(AcmeSearchFilterOperatorEnum searchOperator, List<string?> values);
}