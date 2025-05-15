using System.Reflection;
using API.Domain.Models;
using Xunit;
using Tests.Helpers;

namespace Tests.ModelTests;

public class WifiNetworkTests
{
    [Theory]
    [MemberData(nameof(PropertyHelper.GetNonNullableStringProperties), MemberType = typeof(PropertyHelper))]
    public void NonNullableStringProperty_ShouldThrowArgumentException_WhenIsEmptyOrWhitespace(PropertyInfo property, string value)
    {
        AssertProperty(property, value);
    }
    
    [Theory]
    [MemberData(nameof(PropertyHelper.GetNullableStringProperties), MemberType = typeof(PropertyHelper))]
    public void NullableStringProperty_ShouldThrowArgumentException_WhenIsEmptyOrWhitespace(PropertyInfo property, string value)
    {
        AssertProperty(property, value);
    }
    
    [Theory]
    [MemberData(nameof(PropertyHelper.GetNonNullableIntProperties), MemberType = typeof(PropertyHelper))]
    public void NonNullableIntProperty_ShouldThrowArgumentException_WhenValueIsInvalid(PropertyInfo property, int value)
    {
        AssertProperty(property, value);
    }
    
    [Theory]
    [MemberData(nameof(PropertyHelper.GetNullableIntProperties), MemberType = typeof(PropertyHelper))]
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