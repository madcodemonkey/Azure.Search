using Microsoft.AspNetCore.Mvc;
using Search.Services;

namespace Search.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HotelSearchController : ControllerBase
{
    private readonly IHotelSearchService _hotelSearchService;
    
    /// <summary>Constructor</summary>
    public HotelSearchController(IHotelSearchService hotelSearchService)
    {
        _hotelSearchService = hotelSearchService;
    }
    
    [HttpGet("[action]")]
    public async Task<IActionResult> ShowAll(int pageNumber = 1, int pageSize = 30)
    {
        var response = await _hotelSearchService.GetAllHotelsAsync(pageSize, pageNumber);
        return Ok(response);
    }

    // TODO: Flush out remaining queries and figure out pattern to them.  Potentially pull in filter classes from other project.
     
}