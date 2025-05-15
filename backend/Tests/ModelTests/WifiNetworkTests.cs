using System.Reflection;
using API.Domain.Models;
using Xunit;
using Tests.Helpers;

namespace Tests.ModelTests;

public class WifiNetworkTests
{
    [Theory]
    [MemberData(nameof(WifiNetworkPropertyHelper.GetNonNullableStringProperties), MemberType = typeof(WifiNetworkPropertyHelper))]
    public void NonNullableStringProperty_ShouldThrowArgumentException_WhenIsEmptyOrWhitespace(PropertyInfo property, string value)
    {
        AssertProperty(property, value);
    }
    
    [Theory]
    [MemberData(nameof(WifiNetworkPropertyHelper.GetNullableStringProperties), MemberType = typeof(WifiNetworkPropertyHelper))]
    public void NullableStringProperty_ShouldThrowArgumentException_WhenIsEmptyOrWhitespace(PropertyInfo property, string value)
    {
        AssertProperty(property, value);
    }
    
    [Theory]
    [MemberData(nameof(WifiNetworkPropertyHelper.GetNonNullableIntProperties), MemberType = typeof(WifiNetworkPropertyHelper))]
    public void NonNullableIntProperty_ShouldThrowArgumentException_WhenValueIsInvalid(PropertyInfo property, int value)
    {
        AssertProperty(property, value);
    }
    
    [Theory]
    [MemberData(nameof(WifiNetworkPropertyHelper.GetNullableIntProperties), MemberType = typeof(WifiNetworkPropertyHelper))]
    public void NullableIntProperty_ShouldThrowArgumentException_WhenValueIsInvalid(PropertyInfo property, int? value)
    {
        AssertProperty(property, value);
    }

    private void AssertProperty(PropertyInfo property, object value)
    {
        var wifi = new WifiNetwork();
        
        var exception = Assert.Throws<ArgumentException>(() => property.SetValue(wifi, value));
    
        Assert.Contains($"{property.Name} cannot be set to {value}", exception.Message);
    }
}