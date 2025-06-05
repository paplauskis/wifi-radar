using API.Domain.Dto;
using API.Helpers.Mappers;
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

    [HttpGet("{userId}/favorites")]
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

    [HttpPost("{userId}/favorites")]
    public async Task<IActionResult> AddFavorite([FromRoute] string userId, [FromBody] WifiNetworkDto dto)
    {
        try
        {
            var addedFavorite = await _userFavoriteService.AddUserFavoriteAsync(userId, dto);
            return Ok(addedFavorite);
        }
        catch (Exception e) // specific exception handling will be implemented later
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("{userId}/favorites/{wifiId}")]
    public async Task<IActionResult> DeleteFavorite([FromRoute] string userId, [FromRoute] string wifiId)
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