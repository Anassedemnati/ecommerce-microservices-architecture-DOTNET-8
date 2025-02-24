using Catalog.API.Requests;

namespace Catalog.API.Services;

public interface IProductService
{
    Task<object?> GetProducts();
    Task<object?> GetProduct(string id);
    Task<object?> GetProductByCategory(string categoryName);
    Task<object?> GetProductByName(string name);
    Task<Guid> CreateProduct(ProductRequest product);
    Task<object?> UpdateProduct(ProductRequest product);
    Task<object?> DeleteProduct(string id);
}