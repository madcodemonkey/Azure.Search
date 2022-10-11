using Microsoft.AspNetCore.Mvc;
using Search.Services;

namespace Search.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HotelSynonymController : ControllerBase
{
    private readonly IHotelSynonymService _hotelSynonymService;
   
    /// <summary>Constructor</summary>
    public HotelSynonymController(IHotelSynonymService hotelSynonymService)
    {
        _hotelSynonymService = hotelSynonymService; 
    }

      
    [HttpGet("Maps")]
    public async Task<IActionResult> GetMapList()
    {
        var data = await _hotelSynonymService.GetSynonymMapNamesAsync();
        return Ok(data);
    }

    [HttpGet("Map")]
    public async Task<IActionResult> GetList(string name)
    {
        var data = await _hotelSynonymService.GetSynonymNamesAsync(name);
        return Ok(data);
    }

     
    [HttpPost("Associate")]
    public async Task<IActionResult> AssociateSynonymList()
    {
        await _hotelSynonymService.AssociateSynonymMapToHotelFieldsAsync();
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteSynonymList()
    {
        // Note: Synonyms do NOT belong to an index. They can be used by multiple indexes; however, for this demo I'm only using the
        //       hotel synonyms with the hotel table so I put the delete here.
        var result = await _hotelSynonymService.DeleteAsync();

        return Ok(result ? "Deleted" : "Not Deleted");
    }

    [HttpPost]
    public async Task<IActionResult> CreateSynonymList()
    {
        string message = await _hotelSynonymService.CreateAsync();
        return Ok(message);
    }
}