using API.Domain;
using API.Domain.Models;
using API.Exceptions;
using API.Services.Interfaces.Wifi;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("/api/wifi")]
public class WifiController : ControllerBase
{
    private readonly IWifiReviewService _wifiReviewService;
    private readonly IWifiPasswordSharingService _wifiPasswordSharingService;

    public WifiController(IWifiReviewService wifiReviewService, IWifiPasswordSharingService wifiPasswordSharingService)
    {
        _wifiReviewService = wifiReviewService;
        _wifiPasswordSharingService = wifiPasswordSharingService;
    }

    [HttpGet("reviews")]
    public async Task<IActionResult> GetWifiReviews(
        [FromQuery] string city, 
        [FromQuery] string street, 
        [FromQuery] int buildingNumber)
    {
        try
        {
            var reviews = await _wifiReviewService.GetReviewsAsync(city, street, buildingNumber);
            return Ok(reviews);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (InvalidInputException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "Unexpected server error occurred");
        }
    }

    [HttpPost("reviews")]
    public async Task<IActionResult> AddWifiReview([FromBody] WifiReviewDto wifiReviewDto)
    {
        try
        {
            var review = await _wifiReviewService.AddReviewAsync(wifiReviewDto);
            return Ok(review);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (InvalidInputException e)
        {
            return BadRequest(e.Message);
        }
        catch (ConflictException e)
        {
            return Conflict(e.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "Unexpected server error occurred");
        }
    }

    [HttpGet("{wifiId}/password")]
    public async Task<IActionResult> GetPasswords([FromRoute] string wifiId)
    {
        try
        {
            var passwords = await _wifiPasswordSharingService.GetPasswordsAsync(wifiId);
            return Ok(passwords);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (InvalidInputException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "Unexpected server error occurred");
        }
    }

    [HttpPost("{wifiId}/password")]
    public async Task<IActionResult> AddPassword([FromRoute] string wifiId, [FromBody] PasswordDto passwordDto)
    {
        try
        {
            var password = await _wifiPasswordSharingService.AddPasswordAsync(passwordDto);
            return Ok(password);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (ConflictException e)
        {
            return Conflict(e.Message);
        }
        catch (InvalidInputException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "Unexpected server error occurred");
        }
    }
}
