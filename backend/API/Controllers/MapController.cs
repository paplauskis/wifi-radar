using API.Domain.Exceptions;
using API.Models;
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
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Search([FromQuery] string city, [FromQuery] int? radius) //radius in meters
    {
        var wifis = await _mapService.Search(city, radius);
        if (wifis == null || !wifis.Any())
        {
            return NoContent();
        }

        var response = new ApiResponse<object>
        {
            Success = true,
            Data = wifis,
            Links = new Dictionary<string, string>
            {
                { "self", $"/api/map/search?city={city}&radius={radius}" }
            }
        };
        return Ok(response);
    }

    [HttpGet("coordinates")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCoordinates(
        [FromQuery] string city,
        [FromQuery] string street,
        [FromQuery] int buildingNumber)
    {
        var dto = await _mapService.GetCoordinates(city, street, buildingNumber);
        if (dto == null)
        {
            return NoContent();
        }

        var response = new ApiResponse<object>
        {
            Success = true,
            Data = dto,
            Links = new Dictionary<string, string>
            {
                { "self", $"/api/map/coordinates?city={city}&street={street}&buildingNumber={buildingNumber}" }
            }
        };
        return Ok(response);
    }
}