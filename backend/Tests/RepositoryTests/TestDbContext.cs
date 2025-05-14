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

    public void Dispose()
    {
        _runner.Dispose();
    }
}