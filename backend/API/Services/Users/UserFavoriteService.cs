using API.Domain.Dto;
using API.Domain.Models;
using API.Helpers.Mappers;
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

        public async Task<List<WifiNetworkDto>> GetUserFavoritesAsync(string username)
        {
            var ids = await GetFavoriteNetworkIdsAsync(username);
            if (ids == null || !ids.Any()) return new List<WifiNetworkDto>();

            var filter = Builders<WifiNetwork>.Filter.In(w => w.Id, ids);
            var wifiNetworkList = await _wifiNetworks.Find(filter).ToListAsync();

            var dtoList = new List<WifiNetworkDto>();
            foreach (var wifiNetwork in wifiNetworkList)
            {
                dtoList.Add(Mapper.MapWifiNetworkToDto(wifiNetwork));
            }
            
            return dtoList;
        }

        public async Task<WifiNetworkDto> AddUserFavoriteAsync(string username, WifiNetworkDto wifi)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Username, username);
            var update = Builders<User>.Update.AddToSet(u => u.FavoriteNetworkId, wifi.WifiId);

            await _users.UpdateOneAsync(filter, update);

            
            var existing = await _wifiNetworks.Find(w => w.Id == wifi.WifiId).FirstOrDefaultAsync();
            if (existing == null)
            {
                await _wifiNetworks.InsertOneAsync(Mapper.MapDtoToWifiNetwork(wifi));
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