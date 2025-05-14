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
    protected readonly string _collectionName;
    protected readonly List<TEntity> _testData = JsonHelper.GetPocoObjects<TEntity>();
    protected TestDbContext? _context;
    protected TRepository? _repo;
    protected IMongoCollection<TEntity>? _collection;

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
        _context = new TestDbContext(_collectionName);
        _repo = GetRepository(_context);
        _collection = _context.Database.GetCollection<TEntity>(_collectionName);

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
        _context = new TestDbContext(_collectionName);
        _repo = GetRepository(_context);
        _collection = _context.Database.GetCollection<TEntity>(_collectionName);

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
        _context = new TestDbContext(_collectionName);
        _repo = GetRepository(_context);
        _collection = _context.Database.GetCollection<TEntity>(_collectionName);

        TEntity entityToBeInserted = null!;
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
    
    public abstract Task GetByIdAsync_ShouldReturnCorrectEntity_WhenIdExists(TEntity entity);

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenIdDoesNotExist()
    {
        _context = new TestDbContext(_collectionName);
        _repo = GetRepository(_context);
        _collection = _context.Database.GetCollection<TEntity>(_collectionName);

        var randomId = ObjectId.GenerateNewId().ToString();

        foreach (var entity in _testData)
        {
            await _repo.AddAsync(entity);
        }
        
        var fetchedEntity = await _repo.GetByIdAsync(randomId);
        Assert.Null(fetchedEntity);
        
        _context.Dispose();
    }

    protected abstract TRepository GetRepository(TestDbContext context);
}