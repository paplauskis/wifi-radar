using API.Domain.Models;

namespace API.Services.Interfaces.User;

public interface IUserFavoriteService
{
    Task AddFavoriteAsync(string username, string wifiNetworkId);
    Task RemoveFavoriteAsync(string username, string wifiNetworkId);
    Task<List<string>> GetFavoriteNetworkIdsAsync(string username);
    Task<List<WifiNetwork>> GetFavoriteNetworksAsync(string username);
    Task<bool> IsFavoriteAsync(string username, string wifiNetworkId);
}