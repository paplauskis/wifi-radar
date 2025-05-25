using API.Domain.Models;

namespace API.Services.Interfaces.User;

public interface IUserFavoriteService
{
    Task<List<WifiNetwork>> GetUserFavoritesAsync(string userId);
    Task<WifiNetwork> AddUserFavoriteAsync(string userId, WifiNetwork wifi);
    Task DeleteUserFavoriteAsync(string userId, string wifiId);
}