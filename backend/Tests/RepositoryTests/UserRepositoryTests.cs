using API.Data.Repositories;
using API.Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Xunit;

namespace Tests.RepositoryTests;

public class UserRepositoryTests : BaseRepositoryTests<User, UserRepository>
{
    private const string CollectionName = "Users";
    
    public UserRepositoryTests() : base(CollectionName) {}


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
            _collection = _context.Database.GetCollection<User>(CollectionName);

            var expected = await _repo.AddAsync(_testData[num]);

            var actual = await _collection
                .Find(e => e.Id == expected.Id)
                .FirstOrDefaultAsync();
        
            AssertUserValuesEqual(expected, actual);
        }
        finally
        {
            _context?.Dispose();
        }
    }

    [Theory]
    [MemberData(nameof(ValidObjects))]
    public override async Task GetByIdAsync_ShouldReturnCorrectEntity_WhenIdExists(User entity)
    {
        try
        {
            _context = new TestDbContext(CollectionName);
            _repo = GetRepository(_context);
            _collection = _context.Database.GetCollection<User>(CollectionName);

            var expected = await _repo.AddAsync(entity);
            var actual = await _repo.GetByIdAsync(entity.Id);

            Assert.NotNull(actual);
            AssertUserValuesEqual(expected, actual);
        }
        finally
        {
            _context?.Dispose();
        }
    }

    [Theory]
    [MemberData(nameof(ValidObjects))]
    public async Task GetByUsernameAsync_ShouldReturnCorrectEntity_WhenUsernameExists(User entity)
    {
        try
        {
            _context = new TestDbContext(CollectionName);
            _repo = GetRepository(_context);
            _collection = _context.Database.GetCollection<User>(CollectionName);
        
            var expected = await _repo.AddAsync(entity);
            var actual = await _repo.GetByUsernameAsync(entity.Username);

            Assert.NotNull(actual);
            AssertUserValuesEqual(expected, actual);
        }
        finally
        {
            _context?.Dispose();
        }
    }

    [Fact]
    public async Task GetByUsernameAsync_ShouldReturnNull_WhenUsernameDoesNotExist()
    {
        try
        {
            _context = new TestDbContext(CollectionName);
            _repo = GetRepository(_context);
            _collection = _context.Database.GetCollection<User>(CollectionName);

            var randomUsername = Guid.NewGuid().ToString();
            var fetchedEntity = await _repo.GetByUsernameAsync(randomUsername);

            Assert.Null(fetchedEntity);
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
    public async Task GetByUsernameAsync_ShouldReturnNull_WhenUsernameIsNullOrEmpty(string? username)
    {
        try
        {
            _context = new TestDbContext(CollectionName);
            _repo = GetRepository(_context);
            _collection = _context.Database.GetCollection<User>(CollectionName);
            
            var result = await _repo.GetByUsernameAsync(username);

            Assert.Null(result);
        }
        finally
        {
            _context?.Dispose();
        }
    }

    protected override UserRepository GetRepository(TestDbContext context)
    {
        var services = new ServiceCollection();
        services.AddSingleton(context.Database);
        services.AddScoped<UserRepository>();
        var serviceProvider = services.BuildServiceProvider();
        return serviceProvider.GetRequiredService<UserRepository>();
    }

    private static void AssertUserValuesEqual(User expected, User actual)
    {
        Assert.Equal(expected.Id, actual.Id);
        Assert.Equal(expected.Username, actual.Username);
        Assert.Equal(expected.Password, actual.Password);
    }
}