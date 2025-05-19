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

    [HttpGet("/review/{wifiId}")]
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

    [HttpPost("/review/{wifiId}")]
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
}
// POST /api/wifi/password/{wifiId} (body - password, userId)
// GET api/wifi/password/{wifiId}