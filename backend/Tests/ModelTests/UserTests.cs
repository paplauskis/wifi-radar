using API.Domain.Models;
using Xunit;

namespace Tests.ModelTests;

public class UserTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Username_ShouldThrowArgumentException_WhenUsernameIsNullOrEmpty(string? username)
    {
        var user = new User();
        
        var exception = Assert.Throws<ArgumentException>(() => user.Username = username);
        
        Assert.Contains("Username cannot be null or empty", exception.Message);
    }
}