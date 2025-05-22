using API.Domain.Dto;

namespace API.Services.Interfaces.Auth;

public interface IAuthenticatable
{
    Task<UserLoginResponseDto?> AuthenticateUser(UserLoginRequestDto userRequestDto);
}