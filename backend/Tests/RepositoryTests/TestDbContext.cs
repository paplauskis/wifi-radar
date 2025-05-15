using Tests.Helpers;
using MongoDB.Driver;
using Mongo2Go;
using Newtonsoft.Json;

namespace Tests.RepositoryTests;

public class TestDbContext : IDisposable
{
    private readonly MongoDbRunner _runner;
    private readonly string DatabaseName = "JavaWebService";
    private readonly string _collection;
    
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