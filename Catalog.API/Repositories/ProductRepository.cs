using System.Linq.Expressions;
using Catalog.API.Documents;
using Convey.Persistence.MongoDB;

namespace Catalog.API.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly IMongoRepository<ProductDocument, Guid> _repository;
    
    public ProductRepository(IMongoRepository<ProductDocument, Guid> repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<ProductDocument> GetAsync(Guid id)
    {
        return await _repository.GetAsync(id);
    }
       

    public async Task<Guid> AddAsync(ProductDocument product)
    {
        await _repository.AddAsync(product);
        return product.Id;
    }
    
    public async Task UpdateAsync(ProductDocument product)
    {
        await _repository.UpdateAsync(product);
    }
    
    public async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }
    
    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _repository.ExistsAsync(p => p.Id == id);
    }
    
    public async Task<IEnumerable<ProductDocument>> FindAsync(Expression<Func<ProductDocument, bool>> predicate)
    {
        return await _repository.FindAsync(predicate);
    }

   
    
   
    
    
}
