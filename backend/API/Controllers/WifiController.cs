using API.Domain;
using API.Domain.Models;
using API.Models;
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
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetWifiReviews(
        [FromQuery] string city, 
        [FromQuery] string street, 
        [FromQuery] int buildingNumber)
    {
        var reviews = await _wifiReviewService.GetReviewsAsync(city, street, buildingNumber);
        if (reviews == null || !reviews.Any())
        {
            return NoContent();
        }

        var response = new ApiResponse<object>
        {
            Success = true,
            Data = reviews,
            Links = new Dictionary<string, string>
            {
                { "self", $"/api/wifi/reviews?city={city}&street={street}&buildingNumber={buildingNumber}" }
            }
        };
        return Ok(response);
    }

    [HttpPost("reviews")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddWifiReview([FromBody] WifiReviewDto wifiReviewDto)
    {
        var review = await _wifiReviewService.AddReviewAsync(wifiReviewDto);
        var response = new ApiResponse<object>
        {
            Success = true,
            Data = review,
            Links = new Dictionary<string, string>
            {
                { "self", $"/api/wifi/reviews/{review.Id}" }
            }
        };
        return CreatedAtAction(nameof(GetWifiReviews), new { city = review.City, street = review.Street, buildingNumber = review.BuildingNumber }, response);
    }

    [HttpGet("passwords")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPasswords(
        [FromQuery] string city, 
        [FromQuery] string street, 
        [FromQuery] int buildingNumber)
    {
        var passwords = await _wifiPasswordSharingService.GetPasswordsAsync(city, street, buildingNumber);
        if (passwords == null || !passwords.Any())
        {
            return NoContent();
        }

        var response = new ApiResponse<object>
        {
            Success = true,
            Data = passwords,
            Links = new Dictionary<string, string>
            {
                { "self", $"/api/wifi/passwords?city={city}&street={street}&buildingNumber={buildingNumber}" }
            }
        };
        return Ok(response);
    }

    [HttpPost("passwords")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddPassword([FromBody] PasswordDto passwordDto)
    {
        var password = await _wifiPasswordSharingService.AddPasswordAsync(passwordDto);
        var response = new ApiResponse<object>
        {
            Success = true,
            Data = password,
            Links = new Dictionary<string, string>
            {
                { "self", $"/api/wifi/passwords?city={passwordDto.City}&street={passwordDto.Street}&buildingNumber={passwordDto.BuildingNumber}" }
            }
        };
        return CreatedAtAction(nameof(GetPasswords), new { city = passwordDto.City, street = passwordDto.Street, buildingNumber = passwordDto.BuildingNumber }, response);
    }
}
