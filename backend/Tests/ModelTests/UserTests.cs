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
    
    
    [Theory]
    [InlineData("vfesjnibfsdvnjoaspqiweru0rqiuqropqrmxcmvbnm")]
    [InlineData("aaaaaaaaaaaaaaaaaaaaa")]
    public void Username_ShouldThrowArgumentException_WhenUsernameIsTooLong(string username)
    {
        var user = new User();
        
        var exception = Assert.Throws<ArgumentException>(() => user.Username = username);
        
        Assert.Contains("Username is too long, maximum length is 20", exception.Message);
    }
    
    [Theory]
    [InlineData("123")]
    [InlineData("abc")]
    [InlineData("pass123")]
    [InlineData("")]
    [InlineData("")]
    public void Password_ShouldThrowArgumentException_WhenTooShort(string password)
    {
        var user = new User();
        
        var exception = Assert.Throws<ArgumentException>(() => user.Password = password);
        
        Assert.Contains("Password must be at least 8 characters", exception.Message);
    }
}