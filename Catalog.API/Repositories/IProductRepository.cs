using System.Linq.Expressions;
using Catalog.API.Documents;

namespace Catalog.API.Repositories;

public interface IProductRepository
{
    public Task<Guid> AddAsync(ProductDocument product);
    public Task DeleteAsync(Guid id);
    public Task<bool> ExistsAsync(Guid id);
    public Task<IEnumerable<ProductDocument>> FindAsync(Expression<Func<ProductDocument, bool>> predicate);
    public Task<ProductDocument> GetAsync(Guid id);
    public Task UpdateAsync(ProductDocument product);
    

}