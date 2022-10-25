using Azure.Search.Documents.Models;
using Search.Model;
using System.Text;
using Azure.Search.Documents;

namespace Search.Services;

public abstract class AcmeFilterServiceBase : IAcmeFilterService
{
    private readonly List<IAcmeSearchFilter> _filterList;
    private readonly IAcmeSearchFilter? _securityTrimmingFilter;

    /// <summary>Constructor</summary>
    protected AcmeFilterServiceBase()
    {
        _filterList = RegisterFilters();
        _securityTrimmingFilter = _filterList.FirstOrDefault(w => w.IsSecurityFilter);
    }
    protected virtual int MaximNumberOfFacets => 20;

    /// <summary>Finds a filter by the id it was given when it was created.</summary>
    /// <param name="id">The id to find</param>
    public IAcmeSearchFilter? FindById(int id)
    {
        return _filterList.FirstOrDefault(w => w.Id == id);
    }

    /// <summary>Finds a filter by its name it was given when it was created.</summary>
    /// <param name="fieldName">The field name to find.</param>
    public IAcmeSearchFilter? FindByFieldName(string fieldName)
    {
        return _filterList.FirstOrDefault(w => w.FieldName == fieldName);
    }
    
    /// <summary>Adds facets to your SearchOptions instance.</summary>
    /// <param name="options">The options that need facets</param>
    public void AddFacets(SearchOptions options)
    {
        foreach (var field in _filterList)
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
    public string BuildODataFilter(List<AcmeSearchFilterItem> filters, string[] rolesTheUserIsAssigned)
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

            sbFilter.Append(searchFilter.CreateFilter(filter.Operator, filter.Value));
        }

        // Warning!! If the object of T that you're passing into the Azure Suggest or Azure Search methods does not have the Roles property on it, 
        //           using roles here will do NOTHING!!!  In other words, I didn't want to return roles to the user
        //           so I removed it from by BookDocumentBrief class. Afterwards, this filter STOPPED working!  No ERRORS!
        if (_securityTrimmingFilter != null)
        {
            if (sbFilter.Length > 0)
                sbFilter.Append(" and ");

            sbFilter.Append(_securityTrimmingFilter.CreateFilter(AcmeSearchFilterOperatorEnum.Equal, rolesTheUserIsAssigned));
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

                oneFacet.Items.Add(new AcmeSearchFacetItem()
                {
                    Text = text,
                    Count = item.Count ?? 0,
                    Selected = filters.Any(w => string.Compare(w.Value, text, true) == 0)

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
    protected abstract List<IAcmeSearchFilter> RegisterFilters();
}