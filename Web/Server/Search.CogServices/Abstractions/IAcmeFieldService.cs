using Azure.Search.Documents;
using Azure.Search.Documents.Models;

namespace Search.CogServices;

public interface IAcmeFieldService
{
    /// <summary>Adds facets to your SearchOptions instance.</summary>
    /// <param name="options">The options that need facets</param>
    void AddFacets(SearchOptions options);

    /// <summary>Adds facets to your SearchOptions instance.</summary>
    /// <param name="options">The options that need facets</param>
    /// <param name="clearBeforeAdding">Indicates if we should clear out the highlight fields before adding more.</param>
    void AddHighlightFields(SearchOptions options, bool clearBeforeAdding = true);

    /// <summary>Adds an orderby statement for a field</summary>
    /// <param name="options">The options to add orderby statement to</param>
    /// <param name="fieldId">The id of the field.  If it's not found, you will receive an exception.</param>
    /// <param name="descending">If true, the sort order is descending; otherwise, it ascending</param>
    /// <param name="clearOrderByList">Indicates if you want the order by list cleared before adding a new order by clause.
    /// Most of the time you are only adding one order by statement so this is defaulted to true to clear out anything else that was there.</param>
    void AddOrderBy(SearchOptions options, int fieldId, bool descending, bool clearOrderByList = true);

    /// <summary>Adds an orderby statement for a field</summary>
    /// <param name="options">The options to add orderby statement to</param>
    /// <param name="items">A list of order by fields to add</param>
    /// <param name="clearOrderByList">Indicates if you want the order by list cleared before adding a new order by clause.
    /// Most of the time you are only adding one order by statement so this is defaulted to true to clear out anything else that was there.</param>
    void AddOrderBy(SearchOptions options,  List<AcmeSearchOrderBy> items, bool clearOrderByList = true);

    /// <summary>Adds an search score to the order by statement</summary>
    /// <param name="options">The options to add orderby statement to</param>
    /// <param name="descending">If true, the sort order is descending; otherwise, it ascending</param>
    /// <param name="clearOrderByList">Indicates if you want the order by list cleared before adding a new order by clause.
    /// Most of the time you are only adding one order by statement so this is defaulted to true to clear out anything else that was there.</param>
    void AddScoreToOrderBy(SearchOptions options, bool descending = true, bool clearOrderByList = true);
    
    /// <summary>Builds and OData filter based on user specified filters and the roles that user has been assigned.</summary>
    /// <param name="fieldFilters">Filters to use</param>
    /// <param name="rolesTheUserIsAssigned">Roles assigned to the current user.</param>
    /// <returns>An OData Filter</returns>
    string BuildODataFilter(List<AcmeSearchFilterField> fieldFilters, List<string?> rolesTheUserIsAssigned);

    /// <summary>Avoiding a leaky abstraction by converting Azure Search facets to our format.
    /// Given a list of facets from Azure Search compare them to the filter list we are using and 
    /// mark them as "Selected" so that the user knows they are being used to filter results</summary>
    /// <param name="facets">Facets from an Azure Search call</param>
    /// <param name="fieldFilters">Filters that we are currently using</param>
    /// <returns></returns>
    List<AcmeSearchFacet> ConvertFacets(IDictionary<string, IList<FacetResult>> facets, List<AcmeSearchFilterField> fieldFilters);

    /// <summary>Finds a filter by Azure Index field name it was given when it was created.</summary>
    /// <param name="indexFieldName">The Azure Index field name to find.</param>
    IAcmeSearchField? FindByIndexFieldName(string indexFieldName);

    /// <summary>Finds a filter by C# class property name.  This is the class used to create the index and id decorated with attributes.</summary>
    /// <param name="propFieldName">The C# class property name to find.</param>
    IAcmeSearchField? FindByPropertyFieldName(string propFieldName);

    /// <summary>Finds a filter by the id it was given when it was created.</summary>
    /// <param name="id">The id to find</param>
    IAcmeSearchField? FindById(int id);
}