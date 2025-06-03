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
    
}