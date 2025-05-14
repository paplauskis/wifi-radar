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
    
    public WifiRepositoryTests()
    {
    }
    
    [Fact]
    public async Task AddAsync_ShouldInsertEntityIntoCollection()
    {
        using var context = new TestDbContext(CollectionName);
        var repo = GetRepository(context);
        var collection = context.Database.GetCollection<WifiNetwork>(CollectionName);

        var insertedEntityCount = _testData.Count / 2;

        for (int i = 1; i <= insertedEntityCount; i++)
        {
            var insertedEntity = await repo.AddAsync(_testData[i]);

            var fetchedEntity = await collection
                .Find(e => e.Id == insertedEntity.Id)
                .FirstOrDefaultAsync();
            
            Assert.Equal(insertedEntity.Id, fetchedEntity.Id);
        }
        
        var count = await collection.CountDocumentsAsync(new BsonDocument());
        Assert.Equal(insertedEntityCount, count);
    }

    [Fact]
    public async Task AddAsync_ShouldGenerateNewId_WhenIdIsNull()
    {
        using var context = new TestDbContext(CollectionName);
        var repo = GetRepository(context);
        var collection = context.Database.GetCollection<WifiNetwork>(CollectionName);

        var entityToBeInserted = _testData.First();
        entityToBeInserted.Id = null!;
        await repo.AddAsync(entityToBeInserted);

        var fetchedEntityList = await collection.Find(_ => true).ToListAsync();
        var count = await collection.CountDocumentsAsync(new BsonDocument());
        
        Assert.Single(fetchedEntityList);
        Assert.NotNull(fetchedEntityList[0].Id);
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