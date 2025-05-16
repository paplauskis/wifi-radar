using Xunit;
using API.Domain.Models;
using System;

public class UserValidationTests
{
    [Fact]
    public void Constructor_ValidInput_DoesNotThrow()
    {
     
        var user = new User("TestUser", "Valid1@Password");

   
        Assert.Equal("TestUser", user.Username);
        Assert.Equal("Valid1@Password", user.Password);
    }

    [Theory]
    [InlineData(null, "Valid1@Password", "Username is required.")]
    [InlineData("   ", "Valid1@Password", "Username is required.")]
    [InlineData("User", null, "Password is required.")]
    [InlineData("User", "      ", "Password is required.")]
    [InlineData("User", "short", "Password must include at least one uppercase letter.")]
    [InlineData("User", "NOCAPS1@", "Password must include at least one lowercase letter.")]
    [InlineData("User", "NoSpecial1", "Password must include at least one special character.")]
    [InlineData("User", "Short1!", null)] // Valid example
    [InlineData("User", "WayTooLongPasswordThatExceeds20Chars1!", "Password must be 20 characters or less.")]
    public void Constructor_InvalidInput_ThrowsArgumentException(string username, string password, string expectedMessage)
    {
      
        if (expectedMessage == null)
        {
            var user = new User(username, password);
            Assert.NotNull(user);
        }
        else
        {
            var exception = Assert.Throws<ArgumentException>(() => new User(username, password));
            Assert.Equal(expectedMessage, exception.Message);
        }
    }
}
