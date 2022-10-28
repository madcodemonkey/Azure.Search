using Azure.Search.Documents;
using Azure.Search.Documents.Models;

namespace Search.CogServices;

public interface IAcmeFilterService
{
    /// <summary>Adds facets to your SearchOptions instance.</summary>
    /// <param name="options">The options that need facets</param>
    void AddFacets(SearchOptions options);

    /// <summary>Builds and OData filter based on user specified filters and the roles that user has been assigned.</summary>
    /// <param name="filters">Filters to use</param>
    /// <param name="rolesTheUserIsAssigned">Roles assigned to the current user.</param>
    /// <returns>An OData Filter</returns>
    string BuildODataFilter(List<AcmeSearchFilterItem> filters, string[] rolesTheUserIsAssigned);

    /// <summary>Avoiding a leaky abstraction by converting Azure Search facets to our format.
    /// Given a list of facets from Azure Search compare them to the filter list we are using and 
    /// mark them as "Selected" so that the user knows they are being used to filter results</summary>
    /// <param name="facets">Facets from an Azure Search call</param>
    /// <param name="filters">Filters that we are currently using</param>
    /// <returns></returns>
    List<AcmeSearchFacet> ConvertFacets(IDictionary<string, IList<FacetResult>> facets, List<AcmeSearchFilterItem> filters);

    /// <summary>Finds a filter by its name it was given when it was created.</summary>
    /// <param name="fieldName">The field name to find.</param>
    IAcmeSearchFilter? FindByFieldName(string fieldName);

    /// <summary>Finds a filter by the id it was given when it was created.</summary>
    /// <param name="id">The id to find</param>
    IAcmeSearchFilter? FindById(int id);
}