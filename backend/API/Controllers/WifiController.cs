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
    private readonly IWifiReviewService _wifiSearchService;
    private readonly IWifiPasswordSharingService _wifiPasswordSharingService;

    public WifiController(IWifiReviewService wifiSearchService, IWifiPasswordSharingService wifiPasswordSharingService)
    {
        _wifiSearchService = wifiSearchService;
        _wifiPasswordSharingService = wifiPasswordSharingService;
    }

    [HttpGet("{wifiId}/review")]
    public async Task<IActionResult> GetWifiReviews([FromRoute] string wifiId)
    {
        try
        {
            var reviews = await _wifiSearchService.GetReviewsAsync(wifiId);
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

    [HttpPost("{wifiId}/review")]
    public async Task<IActionResult> AddWifiReview([FromRoute] string wifiId, [FromBody] WifiReviewDto wifiReviewDto)
    {
        try
        {
            wifiReviewDto.WifiId = wifiId;
            var review = await _wifiSearchService.AddReviewAsync(wifiReviewDto);
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
