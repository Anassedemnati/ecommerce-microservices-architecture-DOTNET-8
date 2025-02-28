using Catalog.API.Documents;
using Catalog.API.Dtos;
using Catalog.API.Requests;

namespace Catalog.API.Services;

public interface IProductService
{
    Task<List<ProductDto>> GetProductsAsync();
    Task<ProductDto> GetProductAsync(string id);
    Task<List<ProductDto>> GetProductByCategoryAsync(string categoryName);
    Task<List<ProductDto>> GetProductByNameAsync(string name);
    Task<ProductDto> CreateProductAsync(ProductRequest product);
    Task<ProductDto> UpdateProductAsync(ProductRequest product);
    Task<bool> DeleteProductAsync(string id);
}