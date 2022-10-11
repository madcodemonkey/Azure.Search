using Microsoft.AspNetCore.Mvc;
using Search.Services;

namespace Search.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HotelIndexController : ControllerBase
{
    private readonly IHotelIndexService _hotelIndexService;

    /// <summary>Constructor</summary>
    public HotelIndexController(IHotelIndexService hotelIndexService)
    {
        _hotelIndexService = hotelIndexService;
    }

     
    [HttpGet]
    public async Task<IActionResult> GetList()
    {
        var data = await _hotelIndexService.GetIndexNamesAsync();
        return Ok(data);
    }

    
    [HttpDelete]
    public async Task<IActionResult> DeleteHotelIndex()
    {
        bool created = await _hotelIndexService.DeleteAsync();
        return Ok(created ? "Deleted" : "Not Deleted");
    }

    [HttpPost]
    public async Task<IActionResult> CreateHotelsIndex()
    {
        bool created = await _hotelIndexService.CreateOrUpdateAsync();
        return Ok(created ? "Created" : "Not Created");
    }
}