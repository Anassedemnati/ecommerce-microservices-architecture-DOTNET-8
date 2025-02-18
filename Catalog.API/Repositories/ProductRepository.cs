using Catalog.API.Documents;
using MongoDB.Driver;

namespace Catalog.API.Repositories;

public class ProductRepository : BaseRepository<ProductDocument, Guid>, IProductRepository
{
    public ProductRepository(IMongoDatabase database, ILogger<ProductRepository> logger)
        : base(database, "products")
    {
    }

    
}
