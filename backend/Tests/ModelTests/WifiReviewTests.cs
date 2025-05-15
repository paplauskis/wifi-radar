using System.Reflection;
using API.Domain.Models;
using Tests.Helpers;
using Xunit;

namespace Tests.ModelTests;

public class WifiReviewTests
{
    [Theory]
    [MemberData(nameof(WifiReviewPropertyHelper.GetNonNullableStringProperties), MemberType = typeof(WifiReviewPropertyHelper))]
    public void NonNullableStringProperty_ShouldThrowArgumentException_WhenIsEmptyOrWhitespace(PropertyInfo property, string value)
    {
        AssertProperty(property, value);
    }
    
    [Theory]
    [MemberData(nameof(WifiReviewPropertyHelper.GetNullableStringProperties), MemberType = typeof(WifiReviewPropertyHelper))]
    public void NullableStringProperty_ShouldThrowArgumentException_WhenIsEmptyOrWhitespace(PropertyInfo property, string value)
    {
        AssertProperty(property, value);
    }
    
    [Theory]
    [MemberData(nameof(WifiReviewPropertyHelper.GetNonNullableIntProperties), MemberType = typeof(WifiReviewPropertyHelper))]
    public void NonNullableIntProperty_ShouldThrowArgumentException_WhenValueIsInvalid(PropertyInfo property, int value)
    {
        AssertProperty(property, value);
    }

    [Theory]
    [InlineData("aa")]
    [InlineData("abcd")]
    [InlineData("text text text text text text text text text text text text text text text text text text text text text")]
    public void Text_ShouldThrowArgumentException_WhenTextLengthIsInvalid(string text)
    {
        WifiReview wifi = new WifiReview();
        
        var exception = Assert.Throws<ArgumentException>(() => wifi.Text = text);
    
        Assert.Contains("Text must be between 5 and 100 characters long", exception.Message);
    }

    private void AssertProperty(PropertyInfo property, object value)
    {
        var wifi = new WifiReview();
        
        var exception = Assert.Throws<ArgumentException>(() => property.SetValue(wifi, value));
    
        Assert.Contains($"{property.Name} cannot be set to {value}", exception.Message);
    }
}