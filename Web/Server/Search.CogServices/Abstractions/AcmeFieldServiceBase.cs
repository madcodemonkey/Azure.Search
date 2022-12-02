using Azure.Search.Documents.Models;
using System.Text;
using Azure.Search.Documents;

namespace Search.CogServices;

public abstract class AcmeFieldServiceBase : IAcmeFieldService
{
    public List<IAcmeSearchField> FieldList { get; private set; }

    /// <summary>Constructor</summary>
    protected AcmeFieldServiceBase()
    {
        FieldList = RegisterFields();
    }
    protected virtual int MaximNumberOfFacets => 20;

    /// <summary>Finds a filter by the id it was given when it was created.</summary>
    /// <param name="id">The id to find</param>
    public IAcmeSearchField? FindById(int id)
    {
        return FieldList.FirstOrDefault(w => w.Id == id);
    }

    /// <summary>Finds a filter by Azure Index field name it was given when it was created.</summary>
    /// <param name="indexFieldName">The Azure Index field name to find.</param>
    public IAcmeSearchField? FindByIndexFieldName(string indexFieldName)
    {
        return FieldList.FirstOrDefault(w => w.IndexFieldName == indexFieldName);
    }

    /// <summary>Finds a filter by C# class property name.  This is the class used to create the index and id decorated with attributes.</summary>
    /// <param name="propFieldName">The C# class property name to find.</param>
    public IAcmeSearchField? FindByPropertyFieldName(string propFieldName)
    {
        return FieldList.FirstOrDefault(w => w.PropertyFieldName == propFieldName);
    }

    /// <summary>Finds the security trimming field.</summary>
    public IAcmeSearchField? FindSecurityTrimmingField()
    {
        return FieldList.FirstOrDefault(w => w.IsSecurityFilter);
    }

    /// <summary>Adds an orderby statement for a field</summary>
    /// <param name="options">The options to add orderby statement to</param>
    /// <param name="fieldId">The id of the field.  If it's not found, you will receive an exception.</param>
    /// <param name="descending">If true, the sort order is descending; otherwise, it ascending</param>
    /// <param name="clearOrderByList">Indicates if you want the order by list cleared before adding a new order by clause.
    /// Most of the time you are only adding one order by statement so this is defaulted to true to clear out anything else that was there.</param>
    public void AddOrderBy(SearchOptions options, int fieldId, bool descending, bool clearOrderByList = true)
    {
        var field = FindById(fieldId);
        if (field == null) throw new ArgumentException($"While trying to add an orderby statement, we were unable to find a field with an id of {fieldId}!");
        if (field.IsSortable == false) throw new ArgumentException("You cannot sort by a field that is not marked as sortable!");

        if (clearOrderByList) options.OrderBy.Clear();

        string sortOrder = descending ? "desc" : "asc";
        
        options.OrderBy.Add($"{field.IndexFieldName} {sortOrder}");
    }

    /// <summary>Adds an orderby statement for a field</summary>
    /// <param name="options">The options to add orderby statement to</param>
    /// <param name="items">A list of order by fields to add</param>
    /// <param name="clearOrderByList">Indicates if you want the order by list cleared before adding a new  items to the order by clause.</param>
    public void AddOrderBy(SearchOptions options, List<AcmeSearchOrderBy> items, bool clearOrderByList = true)
    {
        if (clearOrderByList) options.OrderBy.Clear();
        foreach (var item in items)
        {
            AddOrderBy(options, item.FieldId, item.SortDescending, false);
        }
    }

    /// <summary>Adds an search score to the order by statement</summary>
    /// <param name="options">The options to add orderby statement to</param>
    /// <param name="descending">If true, the sort order is descending; otherwise, it ascending</param>
    /// <param name="clearOrderByList">Indicates if you want the order by list cleared before adding a new order by clause.
    /// Most of the time you are only adding one order by statement so this is defaulted to true to clear out anything else that was there.</param>
    public void AddScoreToOrderBy(SearchOptions options, bool descending = true, bool clearOrderByList = true)
    {
        if (clearOrderByList) options.OrderBy.Clear();

        string sortOrder = descending ? "desc" : "asc";
        
        options.OrderBy.Add($"search.score() {sortOrder}");
    }


    /// <summary>Adds facets to your SearchOptions instance.</summary>
    /// <param name="options">The options that need facets</param>
    public void AddFacets(SearchOptions options)
    {
        foreach (var field in FieldList)
        {
            if (field.IsFacetable == false)
                continue;
            // In a faceted search, set an upper limit on unique terms returned in a query.
            // The default is 10, but you can increase or decrease this value using the
            // count parameter on the facet attribute. 
            // See 5th example here https://docs.microsoft.com/en-us/rest/api/searchservice/search-documents#bkmk_examples
            options.Facets.Add($"{field.IndexFieldName},count:{MaximNumberOfFacets}");
        }
    }


    /// <summary>Adds facets to your SearchOptions instance.</summary>
    /// <param name="options">The options that need facets</param>
    /// <param name="clearBeforeAdding">Indicates if we should clear out the highlight fields before adding more.</param>
    public void AddHighlightFields(SearchOptions options, bool clearBeforeAdding = true)
    {
        if (clearBeforeAdding) options.HighlightFields.Clear();
        
        foreach (var field in FieldList)
        {
            if (field.IsHighlighted == false)
                continue;

            options.HighlightFields.Add(field.IndexFieldName);
        }
    }

    /// <summary>Builds and OData filter for Azure Search based on user specified filters and the roles that user has been assigned.</summary>
    /// <param name="fieldFilters">A list of field filter where each represents a grouping of filters for one field.</param>
    /// <param name="rolesTheUserIsAssigned">Roles assigned to the current user.</param>
    /// <returns>An OData Filter</returns>
    public string BuildODataFilter(List<AcmeSearchFilterField> fieldFilters, List<string?> rolesTheUserIsAssigned)
    {
        // All Filters are case SENSITIVE
        // All Filters are case SENSITIVE
        // All Filters are case SENSITIVE
        // All Filters are case SENSITIVE
        // All Filters are case SENSITIVE
        // All Filters are case SENSITIVE

        var sbFilter = new StringBuilder();
        
        for (var index = 0; index < fieldFilters.Count; index++)
        {
            var oneFieldFilter = fieldFilters[index];
            if (index > 0)
            {
                if (fieldFilters[index-1].PeerOperator == AcmeSearchGroupOperatorEnum.And)
                    sbFilter.Append(" and ");
                else sbFilter.Append(" or ");
            }

            // Note: The call to BuildODataFilterForOneFieldFilter will surround the resulting OData filter
            //       with parenthesis if two or more filters are OR'ed together.
            string oneFieldODataFilter = BuildODataFilterForOneFieldFilter(oneFieldFilter);

            sbFilter.Append(oneFieldODataFilter);
        }

        // Warning!! If the object of T that you're passing into the Azure Suggest or Azure Search methods does not have the Roles property on it, 
        //           using roles here will do NOTHING!!!  In other words, I didn't want to return roles to the user
        //           so I removed it from by BookDocumentBrief class. Afterwards, this filter STOPPED working!  No ERRORS!
        if (rolesTheUserIsAssigned.Count > 0)
        {
            var securityTrimmingField = FindSecurityTrimmingField();
            if (securityTrimmingField != null)
            {
                if (sbFilter.Length > 0)
                {
                    if (ShouldSurroundAllTheFieldFiltersWithParenthesis(fieldFilters))
                        sbFilter.SurroundWithParenthesis();
                    
                    sbFilter.Append(" and ");
                }

                sbFilter.Append(securityTrimmingField.CreateFilter(AcmeSearchFilterOperatorEnum.Equal, rolesTheUserIsAssigned));
            }
        }

        return sbFilter.ToString();
    }


    /// <summary>Avoiding a leaky abstraction by converting Azure Search facets to our format.
    /// Given a list of facets from Azure Search compare them to the filter list we are using and 
    /// mark them as "Selected" so that the user knows they are being used to filter results</summary>
    /// <param name="facets">Facets from an Azure Search call</param>
    /// <param name="fieldFilters">A list of field filter where each represents a grouping of filters for one field.</param>
    /// <returns></returns>
    public List<AcmeSearchFacet> ConvertFacets(IDictionary<string, IList<FacetResult>>? facets, List<AcmeSearchFilterField> fieldFilters)
    {
        var result = new List<AcmeSearchFacet>();

        if (facets == null)
            return result;
        
        foreach (KeyValuePair<string, IList<FacetResult>> facet in facets)
        {
            var oneFacet = new AcmeSearchFacet();

            string facetName = facet.Key;
            var searchFilter = FindByIndexFieldName(facetName);

            if (searchFilter == null)
                throw new ArgumentNullException($"Could not find a search filter named '{facetName}'");

            oneFacet.Id = searchFilter.Id;
            oneFacet.DisplayText = searchFilter.DisplayName;

            foreach (FacetResult item in facet.Value)
            {
                var text = item.Value.ToString();

                oneFacet.Items.Add(new AcmeSearchFacetItem
                {
                    Text = text,
                    Count = item.Count ?? 0,
                    Selected = IsFacetSelected(fieldFilters, searchFilter.Id, text)
                });
            }

            result.Add(oneFacet);
        }

        return result;
    }

    /// <summary>Creates an OData filter for one group.</summary>
    /// <param name="fieldFilter">A grouping of filters for one field.</param>
    private string BuildODataFilterForOneFieldFilter(AcmeSearchFilterField fieldFilter)
    {
        var sbGroupFilter = new StringBuilder();

        var searchFilter = FindById(fieldFilter.FieldId);
        if (searchFilter == null)
            throw new ArgumentNullException($"Could not find a search filter with an field id of '{fieldFilter.FieldId}'");
        if (searchFilter.IsSecurityFilter)
            throw new ArgumentException("User is trying to specify a security trimming filter which is illegal!");

        foreach (var filter in fieldFilter.Filters)
        {
            if (sbGroupFilter.Length > 0)
            {
                if (fieldFilter.FiltersOperator == AcmeSearchGroupOperatorEnum.And)
                    sbGroupFilter.Append(" and ");
                else sbGroupFilter.Append(" or ");
            }
         
            sbGroupFilter.Append(searchFilter.CreateFilter(filter.Operator, filter.Values));
        }

        if (fieldFilter.Filters.Count > 1 && fieldFilter.FiltersOperator == AcmeSearchGroupOperatorEnum.Or)
            sbGroupFilter.SurroundWithParenthesis();

        return sbGroupFilter.ToString();
    }

    /// <summary>Indicates if the facet should be selected or not.</summary>
    /// <param name="fieldFilters">A list of field filter where each represents a grouping of filters for one field.</param>
    /// <param name="fieldId">The id of the field associated with the facet</param>
    /// <param name="facetText">The text of the facet item</param>
    private bool IsFacetSelected(List<AcmeSearchFilterField> fieldFilters, int fieldId, string facetText)
    {
        var group = fieldFilters.FirstOrDefault(w => w.FieldId == fieldId);
        if (group == null) return false;

        return group.Filters.Any(w => string.Compare(w.Values[0], facetText, StringComparison.InvariantCultureIgnoreCase) == 0);
    }

    /// <summary>Register all the fields that are marked as IsFilterable or IsFacetable
    /// here.  We do NOT want the client side building filters.  It can only pass in
    /// query text.  This avoids injection attacks where the client can see anything in the
    /// index.  If they could build them, they could avoid security trimming.</summary>
    protected abstract List<IAcmeSearchField> RegisterFields();
    
    /// <summary>Determines if we should surround all the field list filers with parenthesis before adding security trimming</summary>
    /// <param name="fieldFilters">A list of field filter where each represents a grouping of filters for one field.</param>
    private bool ShouldSurroundAllTheFieldFiltersWithParenthesis(List<AcmeSearchFilterField> fieldFilters)
    {
        if (fieldFilters.Count == 0) 
            return false;

        if (fieldFilters.Count == 1)
        {
            if (fieldFilters[0].Filters.Count == 1)
                return false;

            return fieldFilters[0].PeerOperator == AcmeSearchGroupOperatorEnum.Or;
        }

        foreach (var group in fieldFilters)
        {
            if (group.PeerOperator != AcmeSearchGroupOperatorEnum.And)
            {
                return true;
            }
        }

        return false;
    }
}