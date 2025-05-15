using API.Data.Repositories;
using API.Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Tests.Helpers;
using Xunit;

namespace Tests.RepositoryTests;

public class WifiReviewRepositoryTests: BaseRepositoryTests<WifiReview, WifiReviewRepository>
{
    private const string CollectionName = "Reviews";
    // private readonly List<WifiReview> _testData = JsonHelper.GetWifiReviewTestData();
    
    public WifiReviewRepositoryTests() : base(CollectionName) {}

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
            _collection = _context.Database.GetCollection<WifiReview>(CollectionName);

            var expected = await _repo.AddAsync(_testData[num]);

            var actual = await _collection
                .Find(e => e.Id == expected.Id)
                .FirstOrDefaultAsync();

            AssertWifiReviewValuesEqual(expected, actual);
        }
        finally
        {
            _context?.Dispose(); 
        }
    }

    [Theory]
    [MemberData(nameof(ValidObjects))]
    public override async Task GetByIdAsync_ShouldReturnCorrectEntity_WhenIdExists(WifiReview entity)
    {
        try
        {
            _context = new TestDbContext(CollectionName);
            _repo = GetRepository(_context);
            _collection = _context.Database.GetCollection<WifiReview>(CollectionName);
            await _collection.InsertManyAsync(_testData);

            var expected = entity;
            var actual = await _repo.GetByIdAsync(entity.Id);

            Assert.NotNull(actual);
            AssertWifiReviewValuesEqual(expected, actual);
        }
        finally
        {
            _context?.Dispose(); 
        }
    }

    [Theory]
    [MemberData(nameof(ValidObjects))]
    public async Task GetReviewsByWifiIdAsync_ShouldReturnCorrectReviews_WhenWifiIdExists(WifiReview review)
    {
        try
        {
            _context = new TestDbContext(CollectionName);
            _repo = GetRepository(_context);
            _collection = _context.Database.GetCollection<WifiReview>(CollectionName);
            await _collection.InsertManyAsync(_testData);
    
            var expected = _testData
                .Where(e => e.WifiId == review.WifiId)
                .OrderBy(e => e.WifiId)
                .ToList();
            var fetched = await _repo.GetReviewsByWifiIdAsync(review.WifiId);
            var actual = fetched
                .OrderBy(e => e.WifiId)
                .ToList();
    
    
            Assert.NotNull(actual);
            Assert.Equal(expected.Count, actual.Count);
    
            for (int i = 0; i < expected.Count; i++)
            {
                AssertWifiReviewValuesEqual(expected[i], actual[i]);
            }
        }
        finally
        {
            _context?.Dispose(); 
        }
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("RANDOM_ID")]
    [InlineData(null, true)]
    [InlineData("", true)]
    [InlineData("   ", true)]
    [InlineData("RANDOM_ID", true)]
    public async Task GetReviewsByWifiIdAsync_ShouldReturnNoReviews_WhenWifiIdIsInvalid(string? wifiId, bool emptyCollection = false)
    {
        try
        {
            _context = new TestDbContext(CollectionName);
            _repo = GetRepository(_context);
            _collection = _context.Database.GetCollection<WifiReview>(CollectionName);

            if (!emptyCollection)
            {
                await _collection.InsertManyAsync(_testData);
            }
            
            var fetched = await _repo.GetReviewsByWifiIdAsync(wifiId);
            var actual = fetched
                .OrderBy(e => e.WifiId)
                .ToList();

            Assert.Empty(actual);
        }
        finally
        {
            _context?.Dispose(); 
        }
    }

    protected override WifiReviewRepository GetRepository(TestDbContext context)
    {
        var services = new ServiceCollection();
        services.AddSingleton(context.Database);
        services.AddScoped<WifiReviewRepository>();
        var serviceProvider = services.BuildServiceProvider();
        return serviceProvider.GetRequiredService<WifiReviewRepository>();
    }

    private static void AssertWifiReviewValuesEqual(WifiReview expected, WifiReview actual)
    {
        Assert.Equal(expected.Id, actual.Id);
        Assert.Equal(expected.UserId, actual.UserId);
        Assert.Equal(expected.WifiId, actual.WifiId);
        Assert.Equal(expected.Text, actual.Text);
        Assert.Equal(expected.Rating, actual.Rating);   
    }
}