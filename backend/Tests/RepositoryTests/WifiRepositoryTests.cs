using API.Data.Repositories;
using API.Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;
using Tests.Helpers;
using Xunit;

namespace Tests.RepositoryTests;

public class WifiRepositoryTests
{
    private const string CollectionName = "Favorite-WiFi-Spots";
    private readonly List<WifiNetwork> _testData = JsonHelper.GetPocoObjects<WifiNetwork>();
    private TestDbContext? _context;
    private WifiRepository? _repo;
    private IMongoCollection<WifiNetwork>? _collection;
    
    [Fact]
    public async Task AddAsync_ShouldInsertEntityIntoCollection()
    {
        _context = new TestDbContext(CollectionName);
        _repo = GetRepository(_context);
        _collection = _context.Database.GetCollection<WifiNetwork>(CollectionName);

        var insertedEntityCount = _testData.Count / 2;

        for (int i = 1; i <= insertedEntityCount; i++)
        {
            var insertedEntity = await _repo.AddAsync(_testData[i]);

            var fetchedEntity = await _collection
                .Find(e => e.Id == insertedEntity.Id)
                .FirstOrDefaultAsync();
            
            Assert.Equal(insertedEntity.Id, fetchedEntity.Id);
        }
        
        var count = await _collection.CountDocumentsAsync(new BsonDocument());
        Assert.Equal(insertedEntityCount, count);
        
        _context.Dispose();
    }

    [Fact]
    public async Task AddAsync_ShouldGenerateNewId_WhenIdIsNull()
    {
        _context = new TestDbContext(CollectionName);
        _repo = GetRepository(_context);
        _collection = _context.Database.GetCollection<WifiNetwork>(CollectionName);

        var entityToBeInserted = _testData.First();
        entityToBeInserted.Id = null!;
        await _repo.AddAsync(entityToBeInserted);

        var fetchedEntityList = await _collection.Find(_ => true).ToListAsync();
        
        Assert.Single(fetchedEntityList);
        Assert.NotNull(fetchedEntityList[0].Id);
        
        _context.Dispose();
    }

    [Fact]
    public async Task AddAsync_ShouldNotOverwriteId_WhenIdIsAlreadySet()
    {
        _context = new TestDbContext(CollectionName);
        _repo = GetRepository(_context);
        _collection = _context.Database.GetCollection<WifiNetwork>(CollectionName);

        var customId = ObjectId.GenerateNewId().ToString();

        var entityToBeInserted = _testData.First();
        entityToBeInserted.Id = customId;
        await _repo.AddAsync(entityToBeInserted);
        var fetchedEntity = await _collection.Find(e => e.Id == customId).FirstAsync();

        Assert.NotNull(fetchedEntity);
        Assert.Equal(customId, entityToBeInserted.Id);
        Assert.Equal(customId, fetchedEntity.Id);
        
        _context.Dispose();
    }

    private WifiRepository GetRepository(TestDbContext context)
    {
        var services = new ServiceCollection();
        services.AddSingleton(context.Database);
        services.AddScoped<WifiRepository>();
        var serviceProvider = services.BuildServiceProvider();
        return serviceProvider.GetRequiredService<WifiRepository>();
    }
}