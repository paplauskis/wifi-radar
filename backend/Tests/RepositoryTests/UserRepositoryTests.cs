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
            Context = new TestDbContext(CollectionName);
            Repo = GetRepository(Context);
            Collection = Context.Database.GetCollection<User>(CollectionName);

            var expected = await Repo.AddAsync(TestData[num]);

            var actual = await Collection
                .Find(e => e.Id == expected.Id)
                .FirstOrDefaultAsync();
        
            AssertUserValuesEqual(expected, actual);
        }
        finally
        {
            Context?.Dispose();
        }
    }

    [Theory]
    [MemberData(nameof(ValidObjects))]
    public override async Task GetByIdAsync_ShouldReturnCorrectEntity_WhenIdExists(User entity)
    {
        try
        {
            Context = new TestDbContext(CollectionName);
            Repo = GetRepository(Context);
            Collection = Context.Database.GetCollection<User>(CollectionName);
            await Collection.InsertManyAsync(TestData);
            
            var expected = entity;
            var actual = await Repo.GetByIdAsync(entity.Id);

            Assert.NotNull(actual);
            AssertUserValuesEqual(expected, actual);
        }
        finally
        {
            Context?.Dispose();
        }
    }

    [Theory]
    [MemberData(nameof(ValidObjects))]
    public async Task GetByUsernameAsync_ShouldReturnCorrectEntity_WhenUsernameExists(User entity)
    {
        try
        {
            Context = new TestDbContext(CollectionName);
            Repo = GetRepository(Context);
            Collection = Context.Database.GetCollection<User>(CollectionName);
            await Collection.InsertManyAsync(TestData);

            var expected = entity;
            var actual = await Repo.GetByUsernameAsync(entity.Username);

            Assert.NotNull(actual);
            AssertUserValuesEqual(expected, actual);
        }
        finally
        {
            Context?.Dispose();
        }
    }

    [Fact]
    public async Task GetByUsernameAsync_ShouldReturnNull_WhenUsernameDoesNotExist()
    {
        try
        {
            Context = new TestDbContext(CollectionName);
            Repo = GetRepository(Context);
            Collection = Context.Database.GetCollection<User>(CollectionName);

            var randomUsername = Guid.NewGuid().ToString();
            var fetchedEntity = await Repo.GetByUsernameAsync(randomUsername);

            Assert.Null(fetchedEntity);
        }
        finally
        {
            Context?.Dispose();
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
            Context = new TestDbContext(CollectionName);
            Repo = GetRepository(Context);
            Collection = Context.Database.GetCollection<User>(CollectionName);
            
            var result = await Repo.GetByUsernameAsync(username);

            Assert.Null(result);
        }
        finally
        {
            Context?.Dispose();
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