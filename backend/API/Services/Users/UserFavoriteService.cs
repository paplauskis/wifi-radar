using API.Domain.Dto;
using API.Domain.Models;
using API.Helpers.Mappers;
using API.Services.Interfaces.User;
using API.Exceptions;
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
            if (string.IsNullOrWhiteSpace(username))
                throw new InvalidDataException("username cannot be null or empty.");

            var ids = await GetFavoriteNetworkIdsAsync(username);
            if (ids == null || !ids.Any()) return new List<WifiNetworkDto>();

            var filter = Builders<WifiNetwork>.Filter.In(w => w.Id, ids);
            var wifiNetworkList = await _wifiNetworks.Find(filter).ToListAsync();

            if (wifiNetworkList == null || !wifiNetworkList.Any())
                throw new WifiNetworkNotFoundException("None of the user's favorite Wifi networks were found.");

            var dtoList = new List<WifiNetworkDto>();
            foreach (var wifiNetwork in wifiNetworkList)
            {
                dtoList.Add(Mapper.MapWifiNetworkToDto(wifiNetwork));
            }

            return dtoList;
        }

        public async Task<WifiNetworkDto> AddUserFavoriteAsync(string username, WifiNetworkDto wifi)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new InvalidInputException("Username cannot be null or empty.");
            if (wifi == null || string.IsNullOrWhiteSpace(wifi.WifiId))
                throw new InvalidInputException("Wifi data is invalid or missing WifiId.");

            var user = await _users.Find(u => u.Id == username).FirstOrDefaultAsync();
            if (user == null)
                throw new UserNotFoundException(username);

            if (user.FavoriteNetworkId != null && user.FavoriteNetworkId.Contains(wifi.WifiId))
                throw new ConflictException("The same wifi network already is saved by this user");

            var filter = Builders<User>.Filter.Eq(u => u.Id, username);
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
            if (string.IsNullOrWhiteSpace(username))
                throw new InvalidDataException("Username cannot be null or empty.");
            if (string.IsNullOrWhiteSpace(wifiId))
                throw new InvalidDataException("WifiId cannot be null or empty");

            var user = await _users.Find(u => u.Username == username).FirstOrDefaultAsync();
            if (user == null)
                throw new UserNotFoundException(username);

            var filter = Builders<User>.Filter.Eq(u => u.Username, username);
            var update = Builders<User>.Update.Pull(u => u.FavoriteNetworkId, wifiId);
            await _users.UpdateOneAsync(filter, update);
        }

        private async Task<List<string>> GetFavoriteNetworkIdsAsync(string username)
        {
            var user = await _users.Find(u => u.Username == username).FirstOrDefaultAsync();
            if (user == null)
                throw new UserNotFoundException(username);

            return user?.FavoriteNetworkId ?? new List<string>();
        }

        public async Task<bool> IsFavoriteAsync(string username, string wifiId)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new InvalidDataException("Username cannot be null or empty.");
            if (string.IsNullOrWhiteSpace(wifiId))
                throw new InvalidDataException("WifiId cannot be null or empty.");

            var user = await _users.Find(u => u.Username == username).FirstOrDefaultAsync();
            if (user == null)
                throw new UserNotFoundException(username);

            return user?.FavoriteNetworkId?.Contains(wifiId) ?? false;
        }
    }
}