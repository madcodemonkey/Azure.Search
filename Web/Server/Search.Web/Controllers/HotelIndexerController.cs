using Microsoft.AspNetCore.Mvc;
using Search.Services;

namespace Search.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HotelIndexerController : ControllerBase
{
    private readonly IHotelIndexerService _hotelIndexerService;

    /// <summary>Constructor</summary>
    public HotelIndexerController(IHotelIndexerService hotelIndexerService)
    {
        _hotelIndexerService = hotelIndexerService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateHotelsIndex()
    {
        bool created = await _hotelIndexerService.CreateAsync();
        return Ok(created ? "Created" : "Not Created");
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteHotelIndex()
    {
        bool created = await _hotelIndexerService.DeleteAsync();
        return Ok(created ? "Deleted" : "Not Deleted");
    }

    [HttpGet]
    public async Task<IActionResult> GetList()
    {
        var data = await _hotelIndexerService.GetListAsync();
        return Ok(data);
    }

    [HttpPost("Run")]
    public async Task<IActionResult> RunHotelIndexer()
    {
        await _hotelIndexerService.RunAsync();
        return Ok();
    }
}