using Hotel.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelWeb.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SynonymController : ControllerBase
{
    private readonly ISearchSynonymService _synonymService;

    public SynonymController(ISearchSynonymService synonymService)
    {
        _synonymService = synonymService;
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> GetList()
    {
        var data = await _synonymService.GetSynonymMapNamesAsync();
        return Ok(data);
    }

    [HttpPost]
    public IActionResult CreateSynonymList()
    {


        return Ok();
    }
}