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
    
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    [InlineData(9)]
    public async Task AddAsync_ShouldInsertEntityIntoCollection(int num)
    {
        _context = new TestDbContext(CollectionName);
        _repo = GetRepository(_context);
        _collection = _context.Database.GetCollection<WifiNetwork>(CollectionName);

        var insertedEntity = await _repo.AddAsync(_testData[num]);

        var fetchedEntity = await _collection
            .Find(e => e.Id == insertedEntity.Id)
            .FirstOrDefaultAsync();
        
        Assert.Equal(insertedEntity.Id, fetchedEntity.Id);
        Assert.Equal(insertedEntity.UserId, fetchedEntity.UserId);
        Assert.Equal(insertedEntity.Country, fetchedEntity.Country);
        Assert.Equal(insertedEntity.City, fetchedEntity.City);
        Assert.Equal(insertedEntity.PlaceName, fetchedEntity.PlaceName);
        Assert.Equal(insertedEntity.Street, fetchedEntity.Street);
        Assert.Equal(insertedEntity.BuildingNumber, fetchedEntity.BuildingNumber);
        Assert.Equal(insertedEntity.PostalCode, fetchedEntity.PostalCode);
        Assert.Equal(insertedEntity.IsFree, fetchedEntity.IsFree);
        Assert.Equal(insertedEntity.Password, fetchedEntity.Password);
        
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

    [Fact]
    public async Task AddAsync_ShouldThrowException_WhenEntityIsNull()
    {
        _context = new TestDbContext(CollectionName);
        _repo = GetRepository(_context);
        _collection = _context.Database.GetCollection<WifiNetwork>(CollectionName);

        WifiNetwork entityToBeInserted = null!;
        bool exceptionThrown = false;

        try
        {
            await _repo.AddAsync(entityToBeInserted);
            await _collection.Find(_ => true).FirstAsync();
        }
        catch (Exception)
        {
            exceptionThrown = true;
        }
        
        Assert.True(exceptionThrown);
        
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