using API.Domain.Exceptions;
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
    
    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string city, [FromQuery] int? radius) //radius in meters
    {
        try
        {
            var wifis = await _mapService.Search(city, radius);
            return Ok(wifis);
        }
        catch (ArgumentNullException ane)
        {
            return BadRequest(ane.Message);
        }
        catch (ArgumentException ae)
        {
            return BadRequest(ae.Message);
        }
        catch (EmptyResponseException)
        {
            return NoContent();
        }
        catch (Exception e)
        {
            return StatusCode(500, $"Unexpected error occured: {e.Message}");
        }
    }

    [HttpGet("coordinates")]
    public async Task<IActionResult> GetCoordinates(
        [FromQuery] string city,
        [FromQuery] string street,
        [FromQuery] int buildingNumber)
    {
        try
        {
            var dto = await _mapService.GetCoordinates(city, street, buildingNumber);
            return Ok(dto);
        }
        catch (Exception e) // specific exception handling will be implemented later
        {
            return BadRequest(e.Message);
        }
    }
}