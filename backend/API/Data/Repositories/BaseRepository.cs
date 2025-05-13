using API.Data.Repositories.Interfaces;
using API.Domain.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace API.Data.Repositories;

public abstract class BaseRepository<T> : ICrudRepository<T> where T : BaseEntity
{
    protected readonly IMongoCollection<T> _collection;

    public BaseRepository(IMongoDatabase database, string collectionName)
    {
        _collection = database.GetCollection<T>(collectionName);
    }

    public virtual async Task<T?> GetByIdAsync(string id)
    {
        var filter = Builders<T>.Filter.Eq(e => e.Id, id);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        entity.Id ??= ObjectId.GenerateNewId().ToString();
        await _collection.InsertOneAsync(entity);
        return entity;
    }

    public virtual async Task DeleteAsync(T entityToBeDeleted)
    {
        var filter = Builders<T>.Filter.Eq(e => e.Id, entityToBeDeleted.Id);
        await _collection.DeleteOneAsync(filter); 
    }

    public virtual async Task<T> UpdateAsync(T entityToBeUpdated)
    {
        var filter = Builders<T>.Filter.Eq(e => e.Id, entityToBeUpdated.Id);
        await _collection.ReplaceOneAsync(filter, entityToBeUpdated);
        return entityToBeUpdated;
    }
}