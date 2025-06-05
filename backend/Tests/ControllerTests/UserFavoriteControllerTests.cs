using System.Net;
using System.Net.Http.Json;
using System.Text;
using API.Domain.Dto;
using MongoDB.Bson;
using Newtonsoft.Json;
using Tests.Helpers;
using Xunit;

namespace Tests.ControllerTests;

public class UserFavoriteControllerTests
{
    private const string ApiUri = "/api/user";
    
    [Fact]
    public async Task AddFavorite_WithValidData_ShouldReturnCreated()
    {
        await using var factory = new ApiWebApplicationFactory();
        var client = factory.CreateClient();
        
        var user = await CreateSampleUser(client);
        var wifiNetwork = WifiNetworkDtoHelper.GetValidWifiNetworkDto(user);
        
        var addFavoriteContent = new StringContent(JsonConvert.SerializeObject(wifiNetwork), Encoding.UTF8, "application/json");
        var addFavoriteResponse = await client.PostAsync($"{ApiUri}/{user.Id}/favorites", addFavoriteContent);
        var addFavoriteResult = await addFavoriteResponse.Content.ReadAsStringAsync();
    
        Assert.Equal(HttpStatusCode.OK, addFavoriteResponse.StatusCode);
        Assert.Contains(wifiNetwork.WifiId, addFavoriteResult);
        Assert.Contains(wifiNetwork.UserId, addFavoriteResult);
        Assert.Contains(wifiNetwork.City, addFavoriteResult);
        Assert.Contains(wifiNetwork.Name, addFavoriteResult);
        Assert.Contains(wifiNetwork.Street, addFavoriteResult);
        Assert.Contains(wifiNetwork.BuildingNumber.ToString()!, addFavoriteResult);
    }

    [Theory]
    [MemberData(nameof(WifiNetworkDtoHelper.InvalidWifiNetworkDtos), MemberType = typeof(WifiNetworkDtoHelper))]
    public async Task AddFavorite_WithInvalidDtoData_ShouldReturnBadRequest(WifiNetworkDto wifiNetwork)
    {
        await using var factory = new ApiWebApplicationFactory();
        var client = factory.CreateClient();
        
        var user = await CreateSampleUser(client);
        wifiNetwork.UserId = user.Id!;
        
        var addFavoriteContent = new StringContent(JsonConvert.SerializeObject(wifiNetwork), Encoding.UTF8, "application/json");
        var addFavoriteResponse = await client.PostAsync($"{ApiUri}/{user.Id}/favorites", addFavoriteContent);
        var addFavoriteResult = await addFavoriteResponse.Content.ReadAsStringAsync();
        
        Assert.Equal(HttpStatusCode.BadRequest, addFavoriteResponse.StatusCode);
        Assert.Equal($"WifiNetworkDto parameter data is not valid", addFavoriteResult);
    }

    [Theory]
    [InlineData("invalidUserId")]
    [InlineData("71cthn8049")]
    [InlineData("id")]
    public async Task AddFavorite_WithInvalidUserId_ShouldReturnNotFound(string? invalidUserId)
    {
        await using var factory = new ApiWebApplicationFactory();
        var client = factory.CreateClient();
        
        var user = await CreateSampleUser(client);
        var wifiNetwork = WifiNetworkDtoHelper.GetValidWifiNetworkDto(user);
        
        var addFavoriteContent = new StringContent(JsonConvert.SerializeObject(wifiNetwork), Encoding.UTF8, "application/json");
        var addFavoriteResponse = await client.PostAsync($"{ApiUri}/{invalidUserId}/favorites", addFavoriteContent);
        var addFavoriteResult = await addFavoriteResponse.Content.ReadAsStringAsync();
        
        Assert.Equal(HttpStatusCode.NotFound, addFavoriteResponse.StatusCode);
        Assert.Equal($"User ID \"{invalidUserId}\" is not valid", addFavoriteResult);
    }
    
    [Fact]
    public async Task AddFavorite_WithDuplicateWifiNetwork_ShouldReturnConflict()
    {
        await using var factory = new ApiWebApplicationFactory();
        var client = factory.CreateClient();
        
        var user = await CreateSampleUser(client);
        var wifiNetwork = WifiNetworkDtoHelper.GetValidWifiNetworkDto(user);
        
        var addFavoriteContent = new StringContent(JsonConvert.SerializeObject(wifiNetwork), Encoding.UTF8, "application/json");
        await client.PostAsync($"{ApiUri}/{user.Id}/favorites", addFavoriteContent);
        var addFavoriteResponse = await client.PostAsync($"{ApiUri}/{user.Id}/favorites", addFavoriteContent);
        var addFavoriteResult = await addFavoriteResponse.Content.ReadAsStringAsync();
    
        Assert.Equal(HttpStatusCode.Conflict, addFavoriteResponse.StatusCode);
        Assert.Equal("The same wifi network already is saved by this user", addFavoriteResult);
    }
    
    private async Task<UserLoginResponseDto> CreateSampleUser(HttpClient client)
    {
        var user = new UserLoginRequestDto { Username = "sampleUser", Password = "randomPassword123" };
        var json = JsonConvert.SerializeObject(user);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PostAsync($"{ApiUri}/auth/register", content);
        var userResponse = await response.Content.ReadFromJsonAsync<UserLoginResponseDto>();
        
        return userResponse;
    }
}