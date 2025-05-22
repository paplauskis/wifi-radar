using API.Domain.Models;
using API.Services.Interfaces.User;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("/api/user")]
public class UserFavoriteController : ControllerBase
{
    private readonly IUserFavoriteService _userFavoriteService;
    
    public UserFavoriteController(IUserFavoriteService userFavoriteService)
    {
        _userFavoriteService = userFavoriteService;
    }

    [HttpGet("favorites/{userId}")]
    public async Task<IActionResult> GetFavorites([FromRoute] string userId)
    {
        try
        { 
            var favorites = await _userFavoriteService.GetUserFavoritesAsync(userId);
            return Ok(favorites);
        }
        catch (Exception e) // specific exception handling will be implemented later
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("favorites/{userId}")]
    public async Task<IActionResult> AddFavorite([FromRoute] string userId, [FromBody] WifiNetwork wifi)
    {
        try
        { 
            var addedFavorite = await _userFavoriteService.AddUserFavoriteAsync(userId, wifi);
            return Ok(addedFavorite);
        }
        catch (Exception e) // specific exception handling will be implemented later
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("favorites/{userId}")]
    public async Task<IActionResult> DeleteFavorite([FromRoute] string userId, [FromBody] string wifiId)
    {
        try
        { 
            await _userFavoriteService.DeleteUserFavoriteAsync(userId, wifiId);
            return Ok();
        }
        catch (Exception e) // specific exception handling will be implemented later
        {
            return BadRequest(e.Message);
        }
    }
}