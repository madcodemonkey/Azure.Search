using Search.CogServices;
using Search.Model;

namespace Search.Services;

public interface IHotelSuggestorService
{
    /// <summary>Suggest</summary>
    /// <param name="request">A request for a suggestion</param>
    /// <param name="rolesTheUserIsAssigned">Case sensitive list of roles that for search trimming.</param>
    /// <returns>List of suggestions</returns>
    Task<List<HotelSuggestorResult>> SuggestAsync(AcmeSuggestQuery request, List<string> rolesTheUserIsAssigned);
}