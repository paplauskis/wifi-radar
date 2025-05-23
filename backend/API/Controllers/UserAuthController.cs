using API.Domain.Dto;
using API.Domain.Exceptions;
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
    public async Task<IActionResult> Login([FromBody] UserLoginRequestDto userRequestDto)
    {
        try
        {
            var response = await _userAuthService.HandleUserLogin(userRequestDto);
            return Ok(response);
        }
        catch (UnauthorizedAccessException uae)
        {
            return Unauthorized(uae.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, $"Unexpected error occured: {e.Message}");
        }
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserLoginRequestDto userRequestDto)
    {
        try
        {
            var response = await _userAuthService.HandleUserRegistration(userRequestDto);
            return Ok(response);
        }
        catch (UserAlreadyExistsException uaee)
        {
            return Conflict(uaee.Message);
        }
        catch (ArgumentNullException ane)
        {
            return BadRequest(ane.Message);
        }
        catch (ArgumentException ae)
        {
            return BadRequest(ae.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, $"Unexpected error occured: {e.Message}");
        }
    }
}