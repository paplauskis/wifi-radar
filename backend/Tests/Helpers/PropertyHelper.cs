using System.Reflection;
using API.Domain.Models;

namespace Tests.Helpers;

public static class PropertyHelper
{
    public static IEnumerable<object[]> GetNonNullableStringProperties()
        => GetProperties<string>(false, ["", "   ", null], false);
    
    public static IEnumerable<object[]> GetNullableStringProperties()
        => GetProperties<string>(true, ["", "   "], false);
    
    public static IEnumerable<object[]> GetNonNullableIntProperties()
    => GetProperties<int>(false, [0, -1, -10, -67, int.MinValue, int.MaxValue], true);
    
    public static IEnumerable<object[]> GetNullableIntProperties()
        => GetProperties<int?>(true, [0, -1, -10, -67, int.MinValue, int.MaxValue], true);
    
    private static IEnumerable<object[]> GetProperties<T>(bool isNullable, T[] testValues, bool isValueProperty)
    {
        var valuesToTest = testValues;
        List<PropertyInfo> properties;

        if (isValueProperty) 
            properties = GetValueProperties<T>(isNullable);
        else
            properties = GetReferenceProperties<T>(isNullable);
        
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
    
    private static bool IsValuePropertyNullable(PropertyInfo property)
    {
        return Nullable.GetUnderlyingType(property.PropertyType) != null;
    }

    private static List<PropertyInfo> GetValueProperties<T>(bool isNullable)
    {
        return typeof(WifiNetwork)
            .GetProperties()
            .Where(p => p.PropertyType == typeof(T) &&
                        IsValuePropertyNullable(p) == isNullable
            )
            .ToList();
    }
    
    private static List<PropertyInfo> GetReferenceProperties<T>(bool isNullable)
    {
        return typeof(WifiNetwork)
            .GetProperties()
            .Where(p => p.PropertyType == typeof(T) &&
                        IsReferencePropertyNullable(p) == isNullable
            ).ToList();
    }
}