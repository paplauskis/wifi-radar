using System.Text.Json;
using API.Domain.Models;

namespace Tests.Helpers;

public static class JsonHelper
{
    public static List<T> GetPocoObjects<T>()
    {
        var filePath = GetFilePath($"{typeof(T).Name}TestData.json");
        var jsonData = File.ReadAllText(filePath);
        var objectList = JsonSerializer.Deserialize<List<T>>(jsonData);

        return objectList!;
    }
    
    private static string GetFilePath(string file)
    {
        return Path.GetFullPath(Path.Combine(
            AppContext.BaseDirectory,
            "..",
            "..",
            "..",
            "TestData",
            file));
    }
}