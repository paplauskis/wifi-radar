using API.Data.Repositories;
using API.Domain.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using Tests.Helpers;
using Xunit;

namespace Tests.RepositoryTests;

public abstract class BaseRepositoryTests<TEntity, TRepository> 
    where TEntity : BaseEntity 
    where TRepository : BaseRepository<TEntity>
{
    private readonly string _collectionName;
    protected readonly List<TEntity> TestData = JsonHelper.GetPocoObjects<TEntity>();
    protected TestDbContext? Context;
    protected TRepository? Repo;
    protected IMongoCollection<TEntity>? Collection;

    public BaseRepositoryTests(string collectionName)
    {
        _collectionName = collectionName;
    }
    
    public static IEnumerable<object[]> ValidObjects =>
        JsonHelper.GetPocoObjects<TEntity>()
            .Take(10)
            .Select(w => new object[] { w });
    
    public abstract Task AddAsync_ShouldInsertEntityIntoCollection(int num);

    [Fact]
    public async Task AddAsync_ShouldGenerateNewId_WhenIdIsNull()
    {
        try
        {
            Context = new TestDbContext(_collectionName);
            Repo = GetRepository(Context);
            Collection = Context.Database.GetCollection<TEntity>(_collectionName);

            var entityToBeInserted = TestData.First();
            entityToBeInserted.Id = null!;
            await Repo.AddAsync(entityToBeInserted);

            var fetchedEntityList = await Collection.Find(_ => true).ToListAsync();
        
            Assert.Single(fetchedEntityList);
            Assert.NotNull(fetchedEntityList[0].Id);
        }
        finally
        {
            Context?.Dispose();
        }
    }

    [Fact]
    public async Task AddAsync_ShouldNotOverwriteId_WhenIdIsAlreadySet()
    {
        try
        {
            Context = new TestDbContext(_collectionName);
            Repo = GetRepository(Context);
            Collection = Context.Database.GetCollection<TEntity>(_collectionName);

            var customId = ObjectId.GenerateNewId().ToString();

            var entityToBeInserted = TestData.First();
            entityToBeInserted.Id = customId;
            await Repo.AddAsync(entityToBeInserted);
            var fetchedEntity = await Collection.Find(e => e.Id == customId).FirstAsync();

            Assert.NotNull(fetchedEntity);
            Assert.Equal(customId, entityToBeInserted.Id); 
            Assert.Equal(customId, fetchedEntity.Id);
        }
        finally
        {
            Context?.Dispose();
        }
    }

    [Fact]
    public async Task AddAsync_ShouldThrowException_WhenEntityIsNull()
    {
        try
        {
            Context = new TestDbContext(_collectionName);
            Repo = GetRepository(Context);
            Collection = Context.Database.GetCollection<TEntity>(_collectionName);

            TEntity entityToBeInserted = null!;
            bool exceptionThrown = false;

            try
            {
                await Repo.AddAsync(entityToBeInserted);
                await Collection.Find(_ => true).FirstAsync();
            }
            catch (Exception)
            {
                exceptionThrown = true;
            }
        
            Assert.True(exceptionThrown);
        }
        finally
        {
            Context?.Dispose();
        }
    }
    
    public abstract Task GetByIdAsync_ShouldReturnCorrectEntity_WhenIdExists(TEntity entity);

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenIdDoesNotExist()
    {
        try
        {
            Context = new TestDbContext(_collectionName);
            Repo = GetRepository(Context);
            Collection = Context.Database.GetCollection<TEntity>(_collectionName);

            var randomId = ObjectId.GenerateNewId().ToString();

            foreach (var entity in TestData)
            {
                await Repo.AddAsync(entity);
            }
        
            var fetchedEntity = await Repo.GetByIdAsync(randomId);
            Assert.Null(fetchedEntity);
        }
        finally
        {
            Context?.Dispose();
        }
    }

    protected abstract TRepository GetRepository(TestDbContext context);
}