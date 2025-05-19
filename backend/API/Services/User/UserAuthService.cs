using API.Domain;
using API.Services.Interfaces.User;

namespace API.Services.User;

public class UserAuthService : IUserAuthService
{
    public UserAuthService()
    {
        
    }

    public Task<UserLoginResponseDto> HandleUserLogin(UserLoginRequestDto userRequestDto)
    {
        throw new NotImplementedException();
    }

    public Task<UserLoginResponseDto> HandleUserRegistration(UserLoginRequestDto userRequestDto)
    {
        throw new NotImplementedException();
    }
}