using API.Data.Repositories.Interfaces;
using API.Domain.Dto;
using API.Domain.Exceptions;
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
        if (string.IsNullOrEmpty(userRequestDto.Username) || string.IsNullOrEmpty(userRequestDto.Password))
        {
            throw new ArgumentNullException(nameof(userRequestDto), "Username or password cannot be null or empty.");
        }
        
        if (!UserValidator.IsPasswordValid(userRequestDto.Password))
        {
            throw new ArgumentException("Password is invalid; must be at least 6 characters long and contain at least one uppercase letter, one lowercase letter and one number.");
        }
        
        var existingUser = await _userRepository.GetByUsernameAsync(userRequestDto.Username);
        if (existingUser != null) throw new UserAlreadyExistsException($"User with username {userRequestDto.Username} already exists.");

        var newUser = new Domain.Models.User();
        newUser.Username = userRequestDto.Username;
        newUser.Password = _passwordHelper.HashPassword(newUser, userRequestDto.Password);
        await _userRepository.AddAsync(newUser);
        
        return await HandleUserLogin(userRequestDto);
    }
}