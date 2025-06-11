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

    public static string FreeWifiInRadius(int radius, string? lat, string? lon)
    {
        return $"[out:json];\nnode\n  " +
               $"[\"internet_access\"=\"wlan\"]\n  " +
               $"[\"internet_access:fee\"=\"no\"]\n  " +
               $"(around:{radius},{lat},{lon});" +
               $"out body;";
    }

    public static string WifiCoordinates(string city, string street, int buildingNumber)
    {
        return $"[out:json];\nnode\n  " +
               $"[\"internet_access\"=\"wlan\"]\n  " +
               $"[\"internet_access:fee\"=\"no\"]\n  " +
               $"[\"addr:city\"=\"{city}\"]\n" +
               $"[\"addr:street\"=\"{street}\"]\n" +
               $"[\"addr:housenumber\"=\"{buildingNumber}\"];\n" +
               $"out body;";
    }
}

