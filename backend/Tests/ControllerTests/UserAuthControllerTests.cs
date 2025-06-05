using System.Net;
using System.Net.Http.Json;
using System.Text;
using API.Domain.Dto;
using Newtonsoft.Json;
using Xunit;

namespace Tests.ControllerTests;

public class UserAuthControllerTests
{
    private const string ApiUri = "api/user/auth/";
    
    [Theory]
    [InlineData(null, null)]
    [InlineData("jake", "")]
    [InlineData("john", null)]
    [InlineData("Hyena71", "   ")]
    [InlineData("  ", "")]
    [InlineData("", "Password123&")]
    [InlineData("vooDOO", "password123")]
    [InlineData("Irisas", "123Pa")]
    [InlineData("dzojus", "Is33")]
    public async Task Register_WithInvalidCredentials_ReturnsBadRequest(string? username, string? password)
    {
        await using var factory = new ApiWebApplicationFactory();
        var client = factory.CreateClient();

        var user = new UserLoginRequestDto { Username = username, Password = password };
        var json = JsonConvert.SerializeObject(user);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PostAsync($"{ApiUri}register", content);
        var result = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Theory]
    [InlineData("jake", "Jake88")]
    [InlineData("vooDOO", "Password123")]
    [InlineData("Irisas", "QWERTY451yao")]
    [InlineData("dzojus", "VeryGoodPassword987123405")]
    public async Task Register_WithValidCredentials_ReturnsOk(string username, string password)
    {
        await using var factory = new ApiWebApplicationFactory();
        var client = factory.CreateClient();

        var user = new UserLoginRequestDto { Username = username, Password = password };
        var json = JsonConvert.SerializeObject(user);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PostAsync($"{ApiUri}register", content);
        var result = await response.Content.ReadFromJsonAsync<UserLoginResponseDto>();

        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(result.Username, user.Username);
        Assert.NotNull(result.AccessToken);
        Assert.NotEmpty(result.AccessToken);
        Assert.True(result.ExpiresIn > 0);
    }

    [Fact]
    public async Task Register_WithExistingUsername_ReturnsConflict()
    {
        await using var factory = new ApiWebApplicationFactory();
        var client = factory.CreateClient();

        var user = new UserLoginRequestDto { Username = "thisismyusername", Password = "PASSword123" };
        var json = JsonConvert.SerializeObject(user);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        var firstResponse = await client.PostAsync($"{ApiUri}register", content);
        var secondResponse = await client.PostAsync($"{ApiUri}register", content);
        var result = await secondResponse.Content.ReadAsStringAsync();
        
        Assert.Equal(HttpStatusCode.OK, firstResponse.StatusCode);
        Assert.Contains(user.Username, result);
        Assert.Equal(HttpStatusCode.Conflict, secondResponse.StatusCode);
    }

    [Theory]
    [InlineData("jake", "Jake88")]
    [InlineData("vooDOO", "Password123")]
    [InlineData("Irisas", "QWERTY451yao")]
    [InlineData("dzojus", "VeryGoodPassword987123405")]
    public async Task Login_WithValidCredentials_ReturnsOk(string username, string password)
    {
        await using var factory = new ApiWebApplicationFactory();
        var client = factory.CreateClient();

        var user = new UserLoginRequestDto { Username = username, Password = password };
        var json = JsonConvert.SerializeObject(user);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        await client.PostAsync($"{ApiUri}register", content);
        var login = await client.PostAsync($"{ApiUri}login", content);
        var result = await login.Content.ReadAsStringAsync();
        
        Assert.Equal(HttpStatusCode.OK, login.StatusCode);
        Assert.NotEmpty(result);
        Assert.Contains(user.Username, result);
    }

    [Theory]
    [InlineData("invalid_username", "randompassword123")]
    [InlineData("nonexistentusername", "nonexistentpassword")]
    [InlineData("lol", "123")]
    [InlineData("user321", "Word2")]
    public async Task Login_WithInvalidCredentials_ReturnsUnauthorized(string? username, string? password)
    {
        await using var factory = new ApiWebApplicationFactory();
        var client = factory.CreateClient();

        var user = new UserLoginRequestDto { Username = username, Password = password };
        var json = JsonConvert.SerializeObject(user);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        var login = await client.PostAsync($"{ApiUri}login", content);
        
        Assert.Equal(HttpStatusCode.Unauthorized, login.StatusCode);
    }
}