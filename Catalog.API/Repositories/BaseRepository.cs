using Convey.CQRS.Queries;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq.Expressions;

using Convey.Persistence.MongoDB;

namespace Catalog.API.Repositories;

public class BaseRepository<T1, T2> : IBaseRepository<T1, T2> where T1 : class
{
    private readonly IMongoCollection<T1> _collection;

    public BaseRepository(IMongoDatabase database, string collectionName)
    {
        _collection = database.GetCollection<T1>(collectionName);
    }

    public async Task<T1> GetByIdAsync(int id) => await _collection.Find(Builders<T1>.Filter.Eq("_id", id)).FirstOrDefaultAsync();

    public async Task<T1> GetAsync(Expression<Func<T1, bool>> predicate) => await _collection.AsQueryable().FirstOrDefaultAsync(predicate);

    public async Task<IEnumerable<T1>> FindAsync(Expression<Func<T1, bool>> predicate) => await _collection.AsQueryable().Where(predicate).ToListAsync();

    public async Task<PagedResult<T1>> BrowseAsync<TQuery>(Expression<Func<T1, bool>> predicate, TQuery query) where TQuery : PagedQueryBase
    {
        var items = _collection.AsQueryable().Where(predicate);
        return await items.PaginateAsync(query);
    }

    public async Task AddAsync(T1 entity) => await _collection.InsertOneAsync(entity);

    public async Task UpdateAsync(T1 entity, T2 id) => await _collection.ReplaceOneAsync(Builders<T1>.Filter.Eq("_id", id), entity);

    public async Task DeleteAsync(T2 id) => await _collection.DeleteOneAsync(Builders<T1>.Filter.Eq("_id", id));

    public async Task<bool> ExistsAsync(Expression<Func<T1, bool>> predicate) => await _collection.AsQueryable().AnyAsync(predicate);

    public async Task<int> CountAsync() => (int)await _collection.CountDocumentsAsync(Builders<T1>.Filter.Empty);

    public async Task<int> CountAsync(Expression<Func<T1, bool>> predicate) => (int)await _collection.CountDocumentsAsync(predicate);

    public async Task<IEnumerable<T1>> GetAllAsync() => await _collection.AsQueryable().ToListAsync();

    public async Task<IEnumerable<T1>> GetAllAsync(Expression<Func<T1, bool>> predicate) => await _collection.AsQueryable().Where(predicate).ToListAsync();

    public async Task<PagedResult<T1>> BrowseAsync<TQuery>(TQuery query) where TQuery : PagedQueryBase => await _collection.AsQueryable().PaginateAsync(query);

    public async Task<PagedResult<T1>> BrowseAsync(Expression<Func<T1, bool>> predicate, PagedQueryBase query) => await _collection.AsQueryable().Where(predicate).PaginateAsync(query);
}