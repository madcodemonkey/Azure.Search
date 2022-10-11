using Search.Model;

namespace Search.Services;

public interface IHotelSearchService
{
    /// <summary>Returns all hotels</summary>
    /// <param name="numberOfItemsPerPage">The total number of items per page</param>
    /// <param name="pageNumber">Current page number</param>
    Task<SearchResponse<SearchHotel>> GetAllHotelsAsync(int numberOfItemsPerPage, int pageNumber);

}