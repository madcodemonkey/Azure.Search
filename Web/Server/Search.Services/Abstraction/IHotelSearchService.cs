using Search.Model;

namespace Search.Services;

public interface IHotelSearchService
{
    /// <summary>Returns all hotels</summary>
    /// <param name="numberOfItemsPerPage">The total number of items per page</param>
    /// <param name="pageNumber">Current page number</param>
    Task<SearchResponse<SearchHotel>> GetAllHotelsAsync(int numberOfItemsPerPage, int pageNumber);
    
    /// <summary>Suggest</summary>
    /// <param name="request">A request for a suggestion</param>
    /// <param name="rolesTheUserIsAssigned">Case sensitive list of roles that for search trimming.</param>
    /// <returns>List of suggestions</returns>
    Task<List<string>> SuggestAsync(AcmeSearchQuery request, string[] rolesTheUserIsAssigned);

    /// <summary>Search</summary>
    /// <param name="request">A request for a suggestion</param>
    /// <param name="rolesTheUserIsAssigned">Case sensitive list of roles that for search trimming.</param>
    /// <returns>Results from index search</returns>
    Task<AcmeSearchQueryResult<SearchHotel>> SearchAsync(AcmeSearchQuery request, string[] rolesTheUserIsAssigned);

}