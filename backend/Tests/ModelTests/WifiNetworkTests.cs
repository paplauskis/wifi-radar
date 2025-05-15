using System.Reflection;
using API.Domain.Models;
using Xunit;

namespace Tests.ModelTests;

public class WifiNetworkTests
{
    [Theory]
    [MemberData(nameof(GetNonNullableStringProperties))]
    public void NonNullableStringProperty_ShouldThrowArgumentException_WhenIsEmptyOrWhitespace(PropertyInfo property, string value)
    {
        var wifi = new WifiNetwork();
        
        var exception = Assert.Throws<ArgumentException>(() => property.SetValue(wifi, value));
    
        Assert.Contains($"{property.Name} cannot be null, empty or whitespace", exception.Message);
    }
    
    [Theory]
    [MemberData(nameof(GetNullableStringProperties))]
    public void NullableStringProperty_ShouldThrowArgumentException_WhenIsEmptyOrWhitespace(PropertyInfo property, string value)
    {
        var wifi = new WifiNetwork();
        
        var exception = Assert.Throws<ArgumentException>(() => property.SetValue(wifi, value));
    
        Assert.Contains($"{property.Name} cannot be empty or whitespace", exception.Message);
    }

    private static IEnumerable<object[]> GetProperties<T>(bool isNullable, T[] testValues)
    {
        var valuesToTest = testValues;

        var properties = typeof(WifiNetwork)
            .GetProperties()
            .Where(p => p.PropertyType == typeof(T) && IsReferencePropertyNullable(p) == isNullable)
            .ToList();

        foreach (var prop in properties)
        {
            foreach (var value in valuesToTest)
            {
                yield return new object[] { prop, value };
            }
        }
    }

    public static IEnumerable<object[]> GetNonNullableStringProperties()
        => GetProperties<string>(false, ["", "   ", null]);
    
    public static IEnumerable<object[]> GetNullableStringProperties()
        => GetProperties<string>(true, ["", "   "]);

    private static bool IsReferencePropertyNullable(PropertyInfo property)
    {
        var nullabilityContext = new NullabilityInfoContext();
        var nullabilityInfo = nullabilityContext.Create(property);
        return nullabilityInfo.WriteState == NullabilityState.Nullable;
    }
}