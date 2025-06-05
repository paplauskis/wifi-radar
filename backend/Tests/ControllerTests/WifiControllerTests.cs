using System.Net;
using System.Net.Http.Json;
using System.Text;
using API.Domain;
using API.Domain.Dto;
using API.Domain.Models;
using Newtonsoft.Json;
using Tests.Helpers;
using Xunit;

namespace Tests.ControllerTests;

public class WifiControllerTests
{
    private const string ApiUri = "/api/wifi";
    
    [Theory]
    [MemberData(nameof(WifiReviewDtoHelper.InvalidWifiReviewDtos), MemberType = typeof(WifiReviewDtoHelper))]
    public async Task AddWifiReview_WithInvalidData_ShouldReturnBadRequest(WifiReviewDto wifiReviewDto)
    {
        await using var factory = new ApiWebApplicationFactory();
        var client = factory.CreateClient();
        
        var user = await CreateSampleUser(client);
        
        var addReviewContent = new StringContent(JsonConvert.SerializeObject(wifiReviewDto), Encoding.UTF8, "application/json");
        var addWifiReviewResponse = await client.PostAsync($"{ApiUri}/{wifiReviewDto.WifiId}/review", addReviewContent);
        var addWifiReviewResult = await addWifiReviewResponse.Content.ReadAsStringAsync();
    
        Assert.Equal(HttpStatusCode.BadRequest, addWifiReviewResponse.StatusCode);
        Assert.Equal("Invalid data passed", addWifiReviewResult);
    }
    
    [Theory]
    [MemberData(nameof(WifiReviewDtoHelper.ValidWifiReviewDtos), MemberType = typeof(WifiReviewDtoHelper))]
    public async Task AddWifiReview_WithValidData_ShouldReturnOk(WifiReviewDto wifiReviewDto)
    {
        await using var factory = new ApiWebApplicationFactory();
        var client = factory.CreateClient();
        var user = await CreateSampleUser(client);
        wifiReviewDto.UserId = user.Id ?? throw new Exception("User id is null");
        
        var addReviewContent = new StringContent(JsonConvert.SerializeObject(wifiReviewDto), Encoding.UTF8, "application/json");
        var addWifiReviewResponse = await client.PostAsync($"{ApiUri}/{wifiReviewDto.WifiId}/review", addReviewContent);
        
        Assert.Equal(HttpStatusCode.OK, addWifiReviewResponse.StatusCode);
        
        WifiReview? addWifiReviewResult = await addWifiReviewResponse.Content.ReadFromJsonAsync<WifiReview>();
        
        Assert.NotNull(addWifiReviewResult);
        Assert.Equal(wifiReviewDto.UserId, addWifiReviewResult.UserId);
        Assert.Equal(wifiReviewDto.WifiId, addWifiReviewResult.WifiId);
        Assert.Equal(wifiReviewDto.Text, addWifiReviewResult.Text);
        Assert.Equal(wifiReviewDto.Rating, addWifiReviewResult.Rating);
    }
    
    private async Task<UserLoginResponseDto> CreateSampleUser(HttpClient client)
    {
        var user = new UserLoginRequestDto { Username = "sampleUser", Password = "randomPassword123" };
        var json = JsonConvert.SerializeObject(user);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PostAsync($"api/user/auth/register", content);
        var userResponse = await response.Content.ReadFromJsonAsync<UserLoginResponseDto>();
        
        return userResponse;
    }
}