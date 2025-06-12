using API.Domain.Dto;
using API.Models;
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
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetFavorites([FromRoute] string userId)
    {
        var favorites = await _userFavoriteService.GetUserFavoritesAsync(userId);
        var response = new ApiResponse<object>
        {
            Success = true,
            Data = favorites,
            Links = new Dictionary<string, string>
            {
                { "self", $"/api/user/{userId}/favorites" }
            }
        };
        return Ok(response);
    }

    [HttpPost("{userId}/favorites")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddFavorite([FromRoute] string userId, [FromBody] WifiNetworkDto dto)
    {
        var addedFavorite = await _userFavoriteService.AddUserFavoriteAsync(userId, dto);
        var response = new ApiResponse<object>
        {
            Success = true,
            Data = addedFavorite,
            Links = new Dictionary<string, string>
            {
                { "self", $"/api/user/{userId}/favorites/{dto.WifiId}" }
            }
        };
        return CreatedAtAction(nameof(GetFavorites), new { userId }, response);
    }

    [HttpDelete("{userId}/favorites/{wifiId}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteFavorite([FromRoute] string userId, [FromRoute] string wifiId)
    {
        await _userFavoriteService.DeleteUserFavoriteAsync(userId, wifiId);
        return NoContent();
    }
}
