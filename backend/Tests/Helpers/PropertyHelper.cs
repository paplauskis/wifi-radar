using System.Reflection;
using API.Domain.Models;

namespace Tests.Helpers;

internal abstract class PropertyHelper
{
    protected static IEnumerable<object[]> GetProperties<TProperty, TTestObject>(bool isNullable, TProperty[] testValues, bool isValueProperty)
    {
        var valuesToTest = testValues;
        List<PropertyInfo> properties;

        if (isValueProperty) 
            properties = GetValueProperties<TProperty, TTestObject>(isNullable);
        else
            properties = GetReferenceProperties<TProperty, TTestObject>(isNullable);
        
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

    private static List<PropertyInfo> GetValueProperties<TProperty, TTestObject>(bool isNullable)
    {
        return typeof(TTestObject)
            .GetProperties()
            .Where(p => p.PropertyType == typeof(TProperty) &&
                        IsValuePropertyNullable(p) == isNullable
            )
            .ToList();
    }
    
    private static List<PropertyInfo> GetReferenceProperties<TProperty, TTestObject>(bool isNullable)
    {
        return typeof(TTestObject)
            .GetProperties()
            .Where(p => p.PropertyType == typeof(TProperty) &&
                        IsReferencePropertyNullable(p) == isNullable
            ).ToList();
    }
}