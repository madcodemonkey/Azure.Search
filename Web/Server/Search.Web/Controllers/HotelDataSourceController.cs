using Microsoft.AspNetCore.Mvc;
using Search.Services;

namespace Search.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HotelDataSourceController : ControllerBase
{
    private readonly IHotelDataSourceService _hotelDataSourceService;

    /// <summary>Constructor</summary>
    public HotelDataSourceController(IHotelDataSourceService hotelDataSourceService)
    {
        _hotelDataSourceService = hotelDataSourceService;
    }

    [HttpPost]
    public async Task<IActionResult> Create()
    {
        if (await _hotelDataSourceService.CreateAsync())
            return Ok("Created");

        return Ok("Nothing Created.");
    }

    [HttpDelete]
    public async Task<IActionResult> Delete()
    {
        if (await _hotelDataSourceService.DeleteAsync())
            return Ok("Deleted");

        return Ok("Nothing deleted.");
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var data = await _hotelDataSourceService.GetListAsync();
        return Ok(data);
    }
}