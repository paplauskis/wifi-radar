using API.Domain;
using API.Domain.Models;
using API.Services.Interfaces.Wifi;

namespace API.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/wifi")]
public class WifiController : ControllerBase
{
    private readonly IWifiSearchService _wifiSearchService;
    private readonly IWifiPasswordSharingService _wifiPasswordSharingService;
    
    public WifiController(IWifiSearchService wifiSearchService, IWifiPasswordSharingService wifiPasswordSharingService)
    {
        _wifiSearchService = wifiSearchService;
        _wifiPasswordSharingService = wifiPasswordSharingService;
    }

    [HttpGet("review/{wifiId}")]
    public async Task<IActionResult> GetWifiReviews([FromRoute] string wifiId)
    {
        try
        { 
            var reviews = await _wifiSearchService.GetReviewsAsync(wifiId);
            return Ok(reviews);
        }
        catch (Exception e) // specific exception handling will be implemented later
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("review/{wifiId}")]
    public async Task<IActionResult> AddWifiReview([FromRoute] string wifiId, [FromBody] WifiReviewDto wifiReviewDto)
    {
        try
        {
            wifiReviewDto.WifiId = wifiId;
            var review = await _wifiSearchService.AddReviewAsync(wifiReviewDto);
            return Ok(review);
        }
        catch (Exception e) // specific exception handling will be implemented later
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("password/{wifiId}")]
    public async Task<IActionResult> GetPasswords([FromRoute] string wifiId)
    {
        try
        {
            var passwords = await _wifiPasswordSharingService.GetPasswordsAsync(wifiId);
            return Ok(passwords);
        }
        catch (Exception e) // specific exception handling will be implemented later
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("password/{wifiId}")]
    public async Task<IActionResult> AddPassword([FromRoute] string wifiId, [FromBody] PasswordDto passwordDto)
    {
        try
        {
            var password = await _wifiPasswordSharingService.AddPasswordAsync(passwordDto);
            return Ok(password);
        }
        catch (Exception e) // specific exception handling will be implemented later
        {
            return BadRequest(e.Message);
        }
    }
}