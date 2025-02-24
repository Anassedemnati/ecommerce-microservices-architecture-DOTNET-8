using Catalog.API.Repositories;
using Catalog.API.Requests;

namespace Catalog.API.Services;

public class ProductService: IProductService
{
    private readonly IProductRepository _productRepository;
    
    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
    }
    public Task<object?> GetProducts()
    {
        throw new NotImplementedException();
    }

    public Task<object?> GetProduct(string id)
    {
        throw new NotImplementedException();
    }

    public Task<object?> GetProductByCategory(string categoryName)
    {
        throw new NotImplementedException();
    }

    public Task<object?> GetProductByName(string name)
    {
        throw new NotImplementedException();
    }

    public Task<Guid> CreateProduct(ProductRequest product)
    {
        throw new NotImplementedException();
    }

    public Task<object?> UpdateProduct(ProductRequest product)
    {
        throw new NotImplementedException();
    }

    public Task<object?> DeleteProduct(string id)
    {
        throw new NotImplementedException();
    }
}