namespace API.Helpers;

public static class OverpassApi
{
    public static readonly string ApiUrl = "https://overpass-api.de/api/interpreter";
    
    public static string FreeWifiInCity(string city)
    {
        return $"[out:json];\nnode\n  " +
               $"[\"internet_access\"=\"wlan\"]\n  " +
               $"[\"internet_access:fee\"=\"no\"]\n  " +
               $"[\"addr:city\"=\"{city}\"];\n" +
               $"out body;";
    }
}