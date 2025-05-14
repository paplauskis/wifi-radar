using API.Data.Repositories;
using API.Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Xunit;

namespace Tests.RepositoryTests;

public class WifiRepositoryTests : BaseRepositoryTests<WifiNetwork, WifiRepository>
{
    private const string CollectionName = "Favorite-WiFi-Spots";

    public WifiRepositoryTests() : base(CollectionName) {}
    
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
    public override async Task AddAsync_ShouldInsertEntityIntoCollection(int num)
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

    [Theory]
    [MemberData(nameof(ValidObjects))]
    public override async Task GetByIdAsync_ShouldReturnCorrectEntity_WhenIdExists(WifiNetwork entity)
    {
        _context = new TestDbContext(CollectionName);
        _repo = GetRepository(_context);
        _collection = _context.Database.GetCollection<WifiNetwork>(CollectionName);

        var insertedEntity = await _repo.AddAsync(entity);
        var fetchedEntity = await _repo.GetByIdAsync(entity.Id);

        Assert.NotNull(fetchedEntity);
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

    protected override WifiRepository GetRepository(TestDbContext context)
    {
        var services = new ServiceCollection();
        services.AddSingleton(context.Database);
        services.AddScoped<WifiRepository>();
        var serviceProvider = services.BuildServiceProvider();
        return serviceProvider.GetRequiredService<WifiRepository>();
    }
}