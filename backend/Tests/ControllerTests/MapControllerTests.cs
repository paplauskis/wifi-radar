using System.Net;
using System.Net.Http.Json;
using API.Domain.Dto;
using API.Domain.Dto.OverpassDto;
using Xunit;

namespace Tests.ControllerTests;

public class MapControllerTests
{
    [Theory]
    [InlineData("Vilnius")]
    [InlineData("Kaunas")]
    [InlineData("Miami")]
    [InlineData("Berlin")]
    [InlineData("Madrid")]
    [InlineData("Warsaw")]
    public async Task Search_WithValidCity_ReturnsOk(string city)
    {
        await using var factory = new ApiWebApplicationFactory();
        var client = factory.CreateClient();
        
        var response = await client.GetAsync($"api/map/search?city={city}");
        var result = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task Search_WithInvalidCity_ReturnsBadRequest(string city)
    {
        await using var factory = new ApiWebApplicationFactory();
        var client = factory.CreateClient();
        
        var response = await client.GetAsync($"api/map/search?city={city}");
        
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Theory]
    [InlineData("Vilnius", -120)]
    [InlineData("Vilnius", -1)]
    [InlineData("Vilnius", 100001)]
    [InlineData("Vilnius", 560293)]
    [InlineData("Vilnius", int.MaxValue)]
    [InlineData("Vilnius", int.MinValue)]
    public async Task Search_WithInvalidRadius_ReturnsBadRequest(string city, int radius)
    {
        await using var factory = new ApiWebApplicationFactory();
        var client = factory.CreateClient();
        
        var response = await client.GetAsync($"api/map/search?city={city}&radius={radius}");
        var responseText = await response.Content.ReadAsStringAsync();
        
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Contains($"Radius cannot be", responseText);
    }
    
    [Theory]
    [InlineData("wfenolfwjnkmr")]
    [InlineData("citynamecityname")]
    [InlineData("Poland")]
    [InlineData("Germany")]
    public async Task Search_WithNonexistentCity_ReturnsNoContent(string city)
    {
        await using var factory = new ApiWebApplicationFactory();
        var client = factory.CreateClient();
        
        var response = await client.GetAsync($"api/map/search?city={city}");
        var responseText = await response.Content.ReadAsStringAsync();
        
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        Assert.Empty(responseText);
    }
}