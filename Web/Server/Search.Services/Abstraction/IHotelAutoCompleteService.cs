using Azure;
using Azure.Search.Documents.Models;
using Search.CogServices;

namespace Search.Services;

public interface IHotelAutoCompleteService
{
    /// <summary>Autocomplete</summary>
    /// <param name="request">A request for a suggestion</param>
    /// <param name="rolesTheUserIsAssigned">Case sensitive list of roles that for search trimming.</param>
    /// <returns>List of suggestions</returns>
    Task<Response<AutocompleteResults>> AutoCompleteAsync(AcmeSuggestorQuery request, List<string> rolesTheUserIsAssigned);

}