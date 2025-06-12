using API.Domain.Dto;
using API.Models;
using API.Services.Interfaces.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[AllowAnonymous]
[Route("/api/user/auth")]
public class UserAuthController : ControllerBase
{
    private readonly IUserAuthService _userAuthService;
    
    public UserAuthController(IUserAuthService userAuthService)
    {
        _userAuthService = userAuthService;
    }
    
    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login([FromBody] UserLoginRequestDto userRequestDto)
    {
        var response = await _userAuthService.HandleUserLogin(userRequestDto);
        var apiResponse = new ApiResponse<object>
        {
            Success = true,
            Data = response,
            Links = new Dictionary<string, string>
            {
                { "self", "/api/user/auth/login" }
            }
        };
        return Ok(apiResponse);
    }
    
    [HttpPost("register")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Register([FromBody] UserLoginRequestDto userRequestDto)
    {
        var response = await _userAuthService.HandleUserRegistration(userRequestDto);
        var apiResponse = new ApiResponse<object>
        {
            Success = true,
            Data = response,
            Links = new Dictionary<string, string>
            {
                { "self", "/api/user/auth/register" }
            }
        };
        return CreatedAtAction(nameof(Login), null, apiResponse);
    }
}