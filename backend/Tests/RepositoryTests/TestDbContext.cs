using API.Domain.Models;
using MongoDB.Driver;
using Mongo2Go;
using Newtonsoft.Json;

namespace Tests.RepositoryTests;

public class TestDbContext : IDisposable
{
    internal MongoDbRunner _runner;
    internal string DatabaseName = "JavaWebService";
    private string _collection;
    
    internal IMongoDatabase Database { get; }

    public TestDbContext(string collectionName)
    {
        _collection = collectionName;
        _runner = MongoDbRunner.Start(singleNodeReplSet: false);

        MongoClient client = new MongoClient(_runner.ConnectionString);
        Database = client.GetDatabase(DatabaseName);
    }
    
    public void ImportTestData<T>()
    {
        var filePath = GetFilePath($"{typeof(T).Name}TestData.json");
        var jsonData = File.ReadAllText(filePath);
        var objectList = JsonConvert.DeserializeObject<List<T>>(jsonData);
        var collection = Database.GetCollection<T>(_collection);
        
        collection.InsertMany(objectList);
    }
    
    private string GetFilePath(string file)
    {
        return Path.GetFullPath(Path.Combine(
            AppContext.BaseDirectory,
            "..",
            "..",
            "..",
            "TestData",
            file));
    }

    public void Dispose()
    {
        _runner.Dispose();
    }
}