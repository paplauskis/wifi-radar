using API.Data.Repositories;
using API.Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Xunit;

namespace Tests.RepositoryTests;

public class WifiReviewRepositoryTests: BaseRepositoryTests<WifiReview, WifiReviewRepository>
{
    private const string CollectionName = "Reviews";
    
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

            var insertedEntity = await _repo.AddAsync(_testData[num]);

            var fetchedEntity = await _collection
                .Find(e => e.Id == insertedEntity.Id)
                .FirstOrDefaultAsync();

            Assert.Equal(insertedEntity.Id, fetchedEntity.Id);
            Assert.Equal(insertedEntity.UserId, fetchedEntity.UserId);
            Assert.Equal(insertedEntity.WifiId, fetchedEntity.WifiId);
            Assert.Equal(insertedEntity.Text, fetchedEntity.Text);
            Assert.Equal(insertedEntity.Rating, fetchedEntity.Rating);
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

            var insertedEntity = await _repo.AddAsync(entity);
            var fetchedEntity = await _repo.GetByIdAsync(entity.Id);

            Assert.NotNull(fetchedEntity);
            Assert.Equal(insertedEntity.Id, fetchedEntity.Id);
            Assert.Equal(insertedEntity.UserId, fetchedEntity.UserId);
            Assert.Equal(insertedEntity.WifiId, fetchedEntity.WifiId);
            Assert.Equal(insertedEntity.Text, fetchedEntity.Text);
            Assert.Equal(insertedEntity.Rating, fetchedEntity.Rating);   
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
}