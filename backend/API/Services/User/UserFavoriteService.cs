using API.Data.Repositories.Interfaces;
using API.Domain.Models;
using API.Services.Interfaces.User;

namespace API.Services.User;

public class UserFavoriteService : IUserFavoriteService
{
    private readonly IUserRepository _userRepository;
    
    public UserFavoriteService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<List<WifiNetwork>> GetUserFavoritesAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<WifiNetwork> AddUserFavoriteAsync(string userId, WifiNetwork wifi)
    {
        throw new NotImplementedException();
    }

    public Task DeleteUserFavoriteAsync(string userId, string wifiId)
    {
        throw new NotImplementedException();
    }
}