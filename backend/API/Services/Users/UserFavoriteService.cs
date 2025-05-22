using API.Domain.Models;
using API.Services.Interfaces.User;
using MongoDB.Driver;

public class UserFavoriteService : IUserFavoriteService
{
    private readonly IMongoCollection<User> _users;
    private readonly IMongoCollection<WifiNetwork> _wifiNetworks;

    public UserFavoriteService(IMongoDatabase database)
    {
        _users = database.GetCollection<User>("Users");
        _wifiNetworks = database.GetCollection<WifiNetwork>("Favorite-WiFi-Spots");
    }

    public async Task AddFavoriteAsync(string username, string wifiNetworkId)
    {
        var filter = Builders<User>.Filter.Eq(u => u.Username, username);
        var update = Builders<User>.Update.AddToSet(u => u.FavoriteNetworkIds, wifiNetworkId);
        await _users.UpdateOneAsync(filter, update);
    }

    public async Task RemoveFavoriteAsync(string username, string wifiNetworkId)
    {
        var filter = Builders<User>.Filter.Eq(u => u.Username, username);
        var update = Builders<User>.Update.Pull(u => u.FavoriteNetworkIds, wifiNetworkId);
        await _users.UpdateOneAsync(filter, update);
    }

    public async Task<List<string>> GetFavoriteNetworkIdsAsync(string username)
    {
        var user = await _users.Find(u => u.Username == username).FirstOrDefaultAsync();
        return user?.FavoriteNetworkIds ?? new List<string>();
    }

    public async Task<List<WifiNetwork>> GetFavoriteNetworksAsync(string username)
    {
        var ids = await GetFavoriteNetworkIdsAsync(username);
        if (ids == null || !ids.Any()) return new List<WifiNetwork>();

        var filter = Builders<WifiNetwork>.Filter.In(w => w.Id, ids);
        return await _wifiNetworks.Find(filter).ToListAsync();
    }

    public async Task<bool> IsFavoriteAsync(string username, string wifiNetworkId)
    {
        var user = await _users.Find(u => u.Username == username).FirstOrDefaultAsync();
        return user?.FavoriteNetworkIds?.Contains(wifiNetworkId) ?? false;
    }
}