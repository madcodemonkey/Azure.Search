using Hotel.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelWeb.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SynonymController : ControllerBase
{
    private readonly IHotelSynonymService _hotelSynonymService;
   
    /// <summary>Constructor</summary>
    public SynonymController(IHotelSynonymService hotelSynonymService)
    {
        _hotelSynonymService = hotelSynonymService; 
    }
     
    /// <summary>Lists the names of ALL the synonym maps</summary>
    [HttpGet("Maps")]
    public async Task<IActionResult> GetMapNames()
    {
        var data = await _hotelSynonymService.GetSynonymMapNamesAsync();
        return Ok(data);
    }

    /// <summary>Lists all the items in ONE of the synonym maps</summary>
    [HttpGet("Map/{name}")]
    public async Task<IActionResult> GetList(string name)
    {
        var data = await _hotelSynonymService.GetSynonymNamesAsync(name);
        return Ok(data);
    }

    [HttpPost]
    public async Task<IActionResult> CreateSynonymList()
    {
        string message = await _hotelSynonymService.CreateAsync();
        return Ok(message);
    }
}