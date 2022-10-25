using Microsoft.AspNetCore.Mvc;
using Search.Model;
using Search.Services;

namespace Search.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HotelSearchController : ControllerBase
{
    private readonly IHotelSearchService _hotelSearchService;
    private readonly IHotelSuggestorService _hotelSuggestorService;

    /// <summary>Constructor</summary>
    public HotelSearchController(IHotelSearchService hotelSearchService, IHotelSuggestorService hotelSuggestorService)
    {
        _hotelSearchService = hotelSearchService;
        _hotelSuggestorService = hotelSuggestorService;
    }
    
    
    [HttpPost("Suggest/")] 
    public async Task<List<HotelSuggestorResult>> Suggest(AcmeSearchQuery query)
    {
        var result = await _hotelSuggestorService.SuggestAsync(query, GetRoles());

        return result;
    }

    [HttpPost("Search/")] 
    public async Task<AcmeSearchQueryResult<SearchHotel>> Search(AcmeSearchQuery query)
    {
      
        // Reference to paging: https://docs.microsoft.com/en-us/azure/search/tutorial-csharp-paging#extend-your-app-with-numbered-paging
        // Note on how continuation is really used https://stackoverflow.com/questions/33826731/how-to-use-microsoft-azure-search-searchcontinuationtoken
        var result = await _hotelSearchService.SearchAsync(query, GetRoles());
            
        return result;
    }

    
    private string[] GetRoles()
    {
        return new string[] { "admin" };
        //ClaimsPrincipal currentUser = this.User;
        //return currentUser.FindAll(ClaimTypes.Role).Select(s => s.Value.ToLower()).ToArray();
    }
}