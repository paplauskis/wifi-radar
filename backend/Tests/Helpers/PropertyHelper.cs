using System.Reflection;
using API.Domain.Models;

namespace Tests.Helpers;

public static class PropertyHelper
{
    public static IEnumerable<object[]> GetNonNullableStringProperties()
        => GetProperties<string>(false, ["", "   ", null]);
    
    public static IEnumerable<object[]> GetNullableStringProperties()
        => GetProperties<string>(true, ["", "   "]);
    
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
    
    private static bool IsReferencePropertyNullable(PropertyInfo property)
    {
        var nullabilityContext = new NullabilityInfoContext();
        var nullabilityInfo = nullabilityContext.Create(property);
        return nullabilityInfo.WriteState == NullabilityState.Nullable;
    }
}