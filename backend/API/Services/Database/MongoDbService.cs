using MongoDB.Driver;
using MongoDB.Bson;

namespace API.Services.Database;


public class MongoDbService
{
    private readonly IMongoClient _client;
    private readonly IMongoDatabase _database;
    
    public MongoDbService(IConfiguration config)
    {
        var connectionString = config.GetConnectionString("MongoDB");
        var settings = MongoClientSettings.FromConnectionString(connectionString);

        settings.ServerApi = new ServerApi(ServerApiVersion.V1);

        _client = new MongoClient(settings);
        _database = _client.GetDatabase("JavaWebService");
        
        try {
            _database.RunCommand<BsonDocument>(new BsonDocument("ping", 1));
            Console.WriteLine("Pinged your deployment. You successfully connected to MongoDB!");
        } catch (Exception ex) {
            Console.WriteLine(ex);
        }
    }

    public IMongoDatabase GetDatabase()
    {
        return _database;
    }
}