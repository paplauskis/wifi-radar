using API.Data.Repositories.Interfaces;
using API.Domain;
using API.Domain.Dto;
using API.Helpers;
using API.Services.Interfaces.Auth;
using API.Services.Interfaces.User;

namespace API.Services.User;

public class UserAuthService : IUserAuthService
{
    private readonly IAuthenticatable _jwtService;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHelper _passwordHelper;
    
    public UserAuthService(IAuthenticatable jwtService, IUserRepository repo, IPasswordHelper passwordHelper)
    {
        _jwtService = jwtService;
        _userRepository = repo;
        _passwordHelper = passwordHelper;
    }

    public async Task<UserLoginResponseDto> HandleUserLogin(UserLoginRequestDto userRequestDto)
    {
        var user = await _jwtService.AuthenticateUser(userRequestDto);

        if (user == null)
        {
            throw new UnauthorizedAccessException();
        }

        return user;
    }

    public async Task<UserLoginResponseDto> HandleUserRegistration(UserLoginRequestDto userRequestDto)
    {
        throw new NotImplementedException();
    }
}