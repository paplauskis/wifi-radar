using API.Domain.Dto;
using API.Services.Interfaces.User;

namespace API.Services.User;

public class UserAuthService : IUserAuthService
{
    public UserAuthService()
    {
        
    }

    public async Task<UserLoginResponseDto> HandleUserLogin(UserLoginRequestDto userRequestDto)
    {
        throw new NotImplementedException();
    }

    public async Task<UserLoginResponseDto> HandleUserRegistration(UserLoginRequestDto userRequestDto)
    {
        throw new NotImplementedException();
    }
}