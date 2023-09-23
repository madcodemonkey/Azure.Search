using Microsoft.AspNetCore.Mvc;
using Search.Services;
using Search.Web.Models;

namespace Search.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IndexMaintenanceController : ControllerBase
{
    /// <summary>Constructor</summary>
    public IndexMaintenanceController()
    {
    }
 

    [HttpGet("All")]
    public async Task<IActionResult> Get()
    {
        var data = new List<IndexDataDto>()
        {
            new IndexDataDto()
            {
                Created = true,
                Name = "hotels-idx",
                NumberOfDocuments = 29,
                RouteName = "Hotels"
            }
        };

        return Ok(data);
    }
}