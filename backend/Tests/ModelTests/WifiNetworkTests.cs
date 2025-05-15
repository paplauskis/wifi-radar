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
    
        Assert.Contains($"{property.Name} cannot be null or empty or whitespace", exception.Message);
    }

    private static IEnumerable<object[]> GetStringProperties(bool isNullable)
    {
        var valuesToTest = new string[] {"", "   " };

        var properties = typeof(WifiNetwork)
            .GetProperties()
            .Where(p => p.PropertyType == typeof(string) && IsPropertyNullable(p) == isNullable)
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
        => GetStringProperties(false);
    
    public static IEnumerable<object[]> GetNullableStringProperties()
        => GetStringProperties(true);

    private static bool IsPropertyNullable(PropertyInfo property)
    {
        var nullabilityContext = new NullabilityInfoContext();
        var nullabilityInfo = nullabilityContext.Create(property);
        return nullabilityInfo.WriteState == NullabilityState.Nullable;
    }
}