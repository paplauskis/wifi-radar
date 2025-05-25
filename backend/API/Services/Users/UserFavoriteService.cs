using API.Domain.Models;
using API.Services.Interfaces.User;
using MongoDB.Driver;

namespace API.Services.Users
{
    public class UserFavoriteService : IUserFavoriteService
    {
        private readonly IMongoCollection<User> _users;
        private readonly IMongoCollection<WifiNetwork> _wifiNetworks;

        public UserFavoriteService(IMongoDatabase database)
        {
            _users = database.GetCollection<User>("Users");
            _wifiNetworks = database.GetCollection<WifiNetwork>("Favorite-WiFi-Spots");
        }

        public async Task<List<WifiNetwork>> GetUserFavoritesAsync(string username)
        {
            var ids = await GetFavoriteNetworkIdsAsync(username);
            if (ids == null || !ids.Any()) return new List<WifiNetwork>();

            var filter = Builders<WifiNetwork>.Filter.In(w => w.Id, ids);
            return await _wifiNetworks.Find(filter).ToListAsync();
        }

        public async Task<WifiNetwork> AddUserFavoriteAsync(string username, WifiNetwork wifi)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Username, username);
            var update = Builders<User>.Update.AddToSet(u => u.FavoriteNetworkId, wifi.Id);

            await _users.UpdateOneAsync(filter, update);

            
            var existing = await _wifiNetworks.Find(w => w.Id == wifi.Id).FirstOrDefaultAsync();
            if (existing == null)
            {
                await _wifiNetworks.InsertOneAsync(wifi);
            }

            return wifi;
        }

        public async Task DeleteUserFavoriteAsync(string username, string wifiId)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Username, username);
            var update = Builders<User>.Update.Pull(u => u.FavoriteNetworkId, wifiId);
            await _users.UpdateOneAsync(filter, update);
        }

        private async Task<List<string>> GetFavoriteNetworkIdsAsync(string username)
        {
            var user = await _users.Find(u => u.Username == username).FirstOrDefaultAsync();
            return user?.FavoriteNetworkId ?? new List<string>();
        }

        public async Task<bool> IsFavoriteAsync(string username, string wifiId)
        {
            var user = await _users.Find(u => u.Username == username).FirstOrDefaultAsync();
            return user?.FavoriteNetworkId?.Contains(wifiId) ?? false;
        }
    }
}