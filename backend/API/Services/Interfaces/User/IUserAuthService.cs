using API.Domain.Dto;

namespace API.Services.Interfaces.User;

public interface IUserAuthService
{
    Task<UserLoginResponseDto> HandleUserLogin(UserLoginRequestDto userRequestDto);
    Task<UserLoginResponseDto> HandleUserRegistration(UserLoginRequestDto userRequestDto);
}