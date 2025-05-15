using API.Domain.Models;

namespace Tests.Helpers;

internal class WifiReviewPropertyHelper : PropertyHelper
{
    public static IEnumerable<object[]> GetNonNullableStringProperties()
        => GetProperties<string, WifiReview>(false, ["", "   ", null], false);
    
    public static IEnumerable<object[]> GetNullableStringProperties()
        => GetProperties<string, WifiReview>(true, ["", "   "], false);
    
    public static IEnumerable<object[]> GetNonNullableIntProperties()
        => GetProperties<int, WifiReview>(false, [0, -1, -10, -67, int.MinValue, int.MaxValue], true);
}