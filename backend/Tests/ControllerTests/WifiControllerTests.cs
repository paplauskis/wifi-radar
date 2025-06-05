using System.Net;
using System.Net.Http.Json;
using System.Text;
using API.Domain;
using API.Domain.Dto;
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
        
        await CreateSampleUser(client);
        
        var addReviewContent = new StringContent(JsonConvert.SerializeObject(wifiReviewDto), Encoding.UTF8, "application/json");
        var addWifiReviewResponse = await client.PostAsync($"{ApiUri}/{wifiReviewDto.WifiId}/review", addReviewContent);
        var addWifiReviewResult = await addWifiReviewResponse.Content.ReadAsStringAsync();
    
        Assert.Equal(HttpStatusCode.BadRequest, addWifiReviewResponse.StatusCode);
        Assert.Equal("Invalid data passed", addWifiReviewResult);
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