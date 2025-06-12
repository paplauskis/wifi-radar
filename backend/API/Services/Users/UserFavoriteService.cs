using API.Data.Repositories.Interfaces;
using API.Domain.Dto;
using API.Domain.Exceptions;
using API.Domain.Models;
using API.Helpers.Mappers;
using API.Services.Interfaces.User;
using MongoDB.Bson;
using MongoDB.Driver;

namespace API.Services.Users
{
    public class UserFavoriteService : IUserFavoriteService
    {
        private readonly IUserRepository _userRepository;
        private readonly IWifiRepository _wifiRepository;

        public UserFavoriteService(IUserRepository userRepository, IWifiRepository wifiRepository)
        {
            _userRepository = userRepository;
            _wifiRepository = wifiRepository;
        }

        public async Task<List<WifiNetworkDto>> GetUserFavoritesAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new InvalidInputException("Username cannot be null or empty.");
            
            var ids = await GetFavoriteNetworkIdsAsync(userId);
            if (ids == null || !ids.Any()) return new List<WifiNetworkDto>();

            var filter = Builders<WifiNetwork>.Filter.In(w => w.Id, ids);
            var wifiNetworkList = await _wifiRepository.Find(filter);
            
            if (wifiNetworkList == null || !wifiNetworkList.Any())
                throw new NotFoundException("None of the user's favorite Wifi networks were found.");

            var dtoList = new List<WifiNetworkDto>();
            foreach (var wifiNetwork in wifiNetworkList)
            {
                dtoList.Add(Mapper.MapWifiNetworkToDto(wifiNetwork));
            }

            return dtoList;
        }

        public async Task<WifiNetworkDto> AddUserFavoriteAsync(string userId, WifiNetworkDto wifi)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new InvalidInputException("Username cannot be null or empty.");
            if (wifi == null || string.IsNullOrWhiteSpace(wifi.WifiId))
                wifi.WifiId = ObjectId.GenerateNewId().ToString();

            // should check if a WifiNetwork with the same City, Street and BuildingNumber is already saved by the user
            // if the WifiNetwork exists, the doesWifiAlreadyExist variable should be true
            bool doesWifiAlreadyExist = false;
            if (doesWifiAlreadyExist)
                throw new ConflictException("Wifi network is already saved to favorites by this user.");

            var user = await _userRepository.FindById(userId);
            if (user == null)
                throw new NotFoundException($"User with id {userId} could not be found.");
            
            user.FavoriteNetworkId.Add(wifi.WifiId);
            await _userRepository.UpdateOne(user);

            var existing = await _wifiRepository.FindById(wifi.WifiId);
            if (existing == null)
            {
                await _wifiRepository.InsertOne(Mapper.MapDtoToWifiNetwork(wifi));
            }

            return wifi;
        }

        public async Task DeleteUserFavoriteAsync(string userId, string wifiId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new InvalidInputException("Username cannot be null or empty.");
            if (string.IsNullOrWhiteSpace(wifiId))
                throw new InvalidInputException("WifiId cannot be null or empty");

            var user = await _userRepository.FindById(userId);
            if (user == null)
                throw new NotFoundException($"User with id {userId} could not be found.");

            user.FavoriteNetworkId.Remove(wifiId);
            await _userRepository.UpdateOne(user);
        }
        
        private async Task<List<string>> GetFavoriteNetworkIdsAsync(string userId)
        {
            var user = await _userRepository.FindById(userId);
            if (user == null)
                throw new NotFoundException($"User with id {userId} could not be found.");

            return user?.FavoriteNetworkId ?? new List<string>();
        }

        public async Task<bool> IsFavoriteAsync(string username, string wifiId)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new InvalidInputException("Username cannot be null or empty.");
            if (string.IsNullOrWhiteSpace(wifiId))
                throw new InvalidInputException("WifiId cannot be null or empty.");

            var user = await _userRepository.FindById(username);
            if (user == null)
                throw new NotFoundException($"User with username {username} could not be found.");

            return user?.FavoriteNetworkId?.Contains(wifiId) ?? false;
        }
    }
}