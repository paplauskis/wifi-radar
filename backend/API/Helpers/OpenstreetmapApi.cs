namespace API.Helpers;

public static class OpenstreetmapApi
{
    private static readonly string ApiUrl = "https://nominatim.openstreetmap.org/";
    
    public static string CityCenterPoint(string city)
    {
        return $"{ApiUrl}search?q={city}&format=json&limit=1";
    }
}