using API.Domain;
using API.Services.Interfaces.User;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("/api/user/auth")]
public class UserAuthController : ControllerBase
{
    private readonly IUserAuthService _userAuthService;
    
    public UserAuthController(IUserAuthService userAuthService)
    {
        _userAuthService = userAuthService;
    }
    
    [HttpPost("/login")]
    public async Task<IActionResult> Login([FromBody] UserLoginRequestDto userRequestDto)
    {
        try
        {
            var response = await _userAuthService.HandleUserLogin(userRequestDto);
            return Ok(response);
        }
        catch (Exception e) // specific exception handling will be implemented later
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPost("/register")]
    public async Task<IActionResult> Register([FromBody] UserLoginRequestDto userRequestDto)
    {
        try
        {
            var response = await _userAuthService.HandleUserRegistration(userRequestDto);
            return Ok(response);
        }
        catch (Exception e) // specific exception handling will be implemented later
        {
            return BadRequest(e.Message);
        }
    }
}