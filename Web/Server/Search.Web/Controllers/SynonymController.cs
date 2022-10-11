using Search.Services;
using Microsoft.AspNetCore.Mvc;
using Search.Model;

namespace Search.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SynonymController : ControllerBase
{
    private readonly ISearchSynonymService _searchSynonymService;
   
    /// <summary>Constructor</summary>
    public SynonymController(ISearchSynonymService searchSynonymService)
    {
        _searchSynonymService = searchSynonymService;
    }
     
    /// <summary>Lists the names of ALL the synonym maps</summary>
    [HttpGet("Maps")]
    public async Task<IActionResult> GetMapNames()
    {
        var data = await _searchSynonymService.GetSynonymMapNamesAsync();
        return Ok(data);
    }

    /// <summary>Lists all the items in ONE of the synonym maps</summary>
    [HttpGet("Map/{name}")]
    public async Task<IActionResult> GetList(string name)
    {
        var data = await _searchSynonymService.GetSynonymNamesAsync(name);
        return Ok(data);
    }
    
    [HttpDelete("{name}")]
    public async Task<IActionResult> DeleteSynonymList(string name)
    {
        var result = await _searchSynonymService.DeleteAsync(name);

        return Ok(result ? "Deleted" : "Not Deleted");
    }


    [HttpPost]
    public async Task<IActionResult> CreateSynonymList(SynonymCreationRequest request)
    {
        var result = await _searchSynonymService.CreateAsync(request.Name, request.SynonymnList);

        return Ok(result ? "Created" : "Not Created");
    }
}