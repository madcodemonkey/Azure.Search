using Azure.Search.Documents.Models;
using System.Text;
using Azure.Search.Documents;

namespace Search.CogServices;

public abstract class AcmeFieldServiceBase : IAcmeFieldService
{
    public List<IAcmeSearchField> FieldList { get; private set; }
    private readonly IAcmeSearchField? _securityTrimmingField;

    /// <summary>Constructor</summary>
    protected AcmeFieldServiceBase()
    {
        FieldList = RegisterFields();
        _securityTrimmingField = FieldList.FirstOrDefault(w => w.IsSecurityFilter);
    }
    protected virtual int MaximNumberOfFacets => 20;

    /// <summary>Finds a filter by the id it was given when it was created.</summary>
    /// <param name="id">The id to find</param>
    public IAcmeSearchField? FindById(int id)
    {
        return FieldList.FirstOrDefault(w => w.Id == id);
    }

    /// <summary>Finds a filter by its name it was given when it was created.</summary>
    /// <param name="fieldName">The field name to find.</param>
    public IAcmeSearchField? FindByFieldName(string fieldName)
    {
        return FieldList.FirstOrDefault(w => w.FieldName == fieldName);
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
        
        options.OrderBy.Add($"{field.FieldName} {sortOrder}");
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
            options.Facets.Add($"{field.FieldName},count:{MaximNumberOfFacets}");
        }
    }

    /// <summary>Builds and OData filter for Azure Search based on user specified filters and the roles that user has been assigned.</summary>
    /// <param name="filters">Filters to use</param>
    /// <param name="rolesTheUserIsAssigned">Roles assigned to the current user.</param>
    /// <returns>An OData Filter</returns>
    public string BuildODataFilter(List<AcmeSearchFilterItem> filters, List<string> rolesTheUserIsAssigned)
    {
        // All Filters are case SENSITIVE
        // All Filters are case SENSITIVE
        // All Filters are case SENSITIVE
        // All Filters are case SENSITIVE
        // All Filters are case SENSITIVE
        // All Filters are case SENSITIVE

        // Yes.  Need filter build service because I need two pieces of info
        // The exact field name and then a user friendly version of it
        // I also need to know the field type of the index so text can be quoted and numbers should NOT.
        var sbFilter = new StringBuilder();
        foreach (var filter in filters)
        {
            if (sbFilter.Length > 0)
                sbFilter.Append(" and ");
            FindById(filter.Id);

            var searchFilter = FindById(filter.Id);
            if (searchFilter == null)
                throw new ArgumentNullException($"Could not find a search filter with an id of '{filter.Id}'");
            if (searchFilter.IsSecurityFilter)
                throw new ArgumentException("User is trying to specify a security trimming filter which is illegal!");

            sbFilter.Append(searchFilter.CreateFilter(filter.Operator, filter.Values));
        }

        // Warning!! If the object of T that you're passing into the Azure Suggest or Azure Search methods does not have the Roles property on it, 
        //           using roles here will do NOTHING!!!  In other words, I didn't want to return roles to the user
        //           so I removed it from by BookDocumentBrief class. Afterwards, this filter STOPPED working!  No ERRORS!
        if (_securityTrimmingField != null)
        {
            if (sbFilter.Length > 0)
                sbFilter.Append(" and ");

            sbFilter.Append(_securityTrimmingField.CreateFilter(AcmeSearchFilterOperatorEnum.Equal, rolesTheUserIsAssigned));
        }


        return sbFilter.ToString();
    }

    /// <summary>Avoiding a leaky abstraction by converting Azure Search facets to our format.
    /// Given a list of facets from Azure Search compare them to the filter list we are using and 
    /// mark them as "Selected" so that the user knows they are being used to filter results</summary>
    /// <param name="facets">Facets from an Azure Search call</param>
    /// <param name="filters">Filters that we are currently using</param>
    /// <returns></returns>
    public List<AcmeSearchFacet> ConvertFacets(IDictionary<string, IList<FacetResult>>? facets, List<AcmeSearchFilterItem> filters)
    {
        var result = new List<AcmeSearchFacet>();

        if (facets == null)
            return result;
        
        foreach (KeyValuePair<string, IList<FacetResult>> facet in facets)
        {
            var oneFacet = new AcmeSearchFacet();

            string facetName = facet.Key;
            var searchFilter = FindByFieldName(facetName);

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
                    Selected = filters.Any(w => string.Compare(w.Values[0], text, true) == 0)

                });
            }

            result.Add(oneFacet);
        }

        return result;
    }

    /// <summary>Register all the fields that are marked as IsFilterable or IsFacetable
    /// here.  We do NOT want the client side building filters.  It can only pass in
    /// query text.  This avoids injection attacks where the client can see anything in the
    /// index.  If they could build them, they could avoid security trimming.</summary>
    protected abstract List<IAcmeSearchField> RegisterFields();
}