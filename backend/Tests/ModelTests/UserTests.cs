using API.Domain.Models;
using Xunit;

namespace Tests.ModelTests;

public class UserTests
{
    [Theory]
    [InlineData(null, "Username is required.")]
    [InlineData("", "Username is required.")]
    [InlineData("   ", "Username is required.")]
    public void Username_ShouldThrowArgumentException_WhenUsernameIsNullOrEmpty(string? username, string expectedMessage)
    {
        var exception = Assert.Throws<ArgumentException>(() =>
        {
            var user = new User
            {
                Username = username, // this line throws
                Password = "ValidPass1!"
            };
        });

        Assert.Contains(expectedMessage, exception.Message);
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
    [InlineData("123", "Password must be at least 8 characters")]
    [InlineData("abc", "Password must be at least 8 characters")]
    [InlineData("pass123", "Password must be at least 8 characters")]
    [InlineData("", "Password is required.")]
    [InlineData("   ", "Password is required.")]
    [InlineData("Dwq", "Password must be at least 8 characters")]
    public void Password_ShouldThrowArgumentException_WhenTooShort(string password, string expectedMessage)
    {
        var user = new User
        {
            Username = "ValidUser",
            Password = password
        };

        var exception = Assert.Throws<ArgumentException>(() => user.Validate());

        Assert.Contains(expectedMessage, exception.Message);
    }
}