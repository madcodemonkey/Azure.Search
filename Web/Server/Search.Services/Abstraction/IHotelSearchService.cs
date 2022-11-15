using Azure.Search.Documents.Models;
using Search.CogServices;
using Search.Model;

namespace Search.Services;

public interface IHotelSearchService
{
    /// <summary>Search</summary>
    /// <param name="request">The request from the user.</param>
    /// <param name="rolesTheUserIsAssigned">Case sensitive list of roles that for search trimming.</param>
    /// <returns>Results from index search</returns>
    Task<AcmeSearchQueryResult<SearchResult<HotelDocument>>> SearchAsync(AcmeSearchQuery request, List<string> rolesTheUserIsAssigned);
}