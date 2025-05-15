using System.Diagnostics.CodeAnalysis;
using API.Data.Repositories;
using API.Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Tests.Helpers;
using Xunit;

namespace Tests.RepositoryTests;

public class WifiRepositoryTests : BaseRepositoryTests<WifiNetwork, WifiRepository>
{
    private const string CollectionName = "Favorite-WiFi-Spots";
    // private readonly List<WifiNetwork> _testData = JsonHelper.GetWifiNetworkTestData();
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
        try
        {
            _context = new TestDbContext(CollectionName);
            _repo = GetRepository(_context);
            _collection = _context.Database.GetCollection<WifiNetwork>(CollectionName);

            var expected = await _repo.AddAsync(_testData[num]);

            var actual = await _collection
                .Find(e => e.Id == expected.Id)
                .FirstOrDefaultAsync();
        
            AssertWifiNetworkValuesEqual(expected, actual);
        }
        finally
        {
            _context?.Dispose();
        }
    }

    [Theory]
    [MemberData(nameof(ValidObjects))]
    public override async Task GetByIdAsync_ShouldReturnCorrectEntity_WhenIdExists(WifiNetwork entity)
    {
        try
        {
            _context = new TestDbContext(CollectionName);
            _repo = GetRepository(_context);
            _collection = _context.Database.GetCollection<WifiNetwork>(CollectionName);
            await _collection.InsertManyAsync(_testData);

            var expected = entity;
            var actual = await _repo.GetByIdAsync(entity.Id);

            Assert.NotNull(actual);
            AssertWifiNetworkValuesEqual(expected, actual);
        }
        finally
        {
            _context?.Dispose();
        }
    }

    [Theory]
    [InlineData("Vilnius", 5)]
    [InlineData("Kaunas", 3)]
    [InlineData("Klaipėda", 2)]
    [InlineData("Alytus", 0)]
    [InlineData("", 0)]
    [InlineData("  ", 0)]
    [InlineData(null, 0)]
    public async Task GetByCityAsync_ShouldReturnCorrectEntities_WhenCityMatches(string? city, int expectedCount)
    {
        try
        {
            _context = new TestDbContext(CollectionName);
            _repo = GetRepository(_context);
            _collection = _context.Database.GetCollection<WifiNetwork>(CollectionName);

            List<WifiNetwork> correctEntityList = [];

            foreach (var entity in _testData)
            {
                if(entity.City == city) correctEntityList.Add(entity);
                await _collection.InsertOneAsync(entity);
            }

            var entityList = await _repo.GetByCityAsync(city);
            
            var actualEntityList = entityList.OrderBy(e => e.Id).ToList();
            var expectedEntityList = correctEntityList.OrderBy(e => e.Id).ToList();

            Assert.Equal(entityList.Count, expectedCount);

            for (int i = 0; i < expectedCount; i++)
            {
                AssertWifiNetworkValuesEqual(expectedEntityList[i], actualEntityList[i]);
            }
        }
        finally
        {
            _context?.Dispose();
        }
    }

    protected override WifiRepository GetRepository(TestDbContext context)
    {
        var services = new ServiceCollection();
        services.AddSingleton(context.Database);
        services.AddScoped<WifiRepository>();
        var serviceProvider = services.BuildServiceProvider();
        return serviceProvider.GetRequiredService<WifiRepository>();
    }

    private static void AssertWifiNetworkValuesEqual(WifiNetwork expected, WifiNetwork actual)
    {
        Assert.Equal(expected.Id, actual.Id);
        Assert.Equal(expected.UserId, actual.UserId);
        Assert.Equal(expected.Country, actual.Country);
        Assert.Equal(expected.City, actual.City);
        Assert.Equal(expected.PlaceName, actual.PlaceName);
        Assert.Equal(expected.Street, actual.Street);
        Assert.Equal(expected.BuildingNumber, actual.BuildingNumber);
        Assert.Equal(expected.PostalCode, actual.PostalCode);
        Assert.Equal(expected.IsFree, actual.IsFree);
        Assert.Equal(expected.Password, actual.Password);
    }
}