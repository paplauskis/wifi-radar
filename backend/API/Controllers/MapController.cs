using API.Exceptions;
using API.Services.Interfaces.Map;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("/api/map")]
public class MapController : ControllerBase
{
    private readonly IMapService _mapService;

    public MapController(IMapService mapService)
    {
        _mapService = mapService;
    }

    [HttpGet]
    public async Task<IActionResult> GetNearbyWifi([FromQuery] string city, [FromQuery] int? radius)
    {
        try
        {
            var wifis = await _mapService.Search(city, radius);
            return Ok(wifis);
        }
        catch (InvalidInputException e)
        {
            return BadRequest(e.Message);
        }
        catch (EmptyResponseException)
        {
            return NoContent();
        }
        catch (Exception)
        {
            return StatusCode(500, "Unexpected error occurred.");
        }
    }
}
