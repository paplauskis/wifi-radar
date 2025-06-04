using System.Net;
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
}