using Catalog.API.Documents;
using Catalog.API.Dtos;
using Catalog.API.Extensions;
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
    public async Task<List<ProductDto>> GetProductsAsync()
    {
        var res =  await _productRepository
            .FindAsync(p => true)
            .ConfigureAwait(false);
        
        return res.ToProductDtoList();
    
    }

    public async Task<ProductDto> GetProductAsync(string id)
    {
        var res = await _productRepository
            .GetAsync(Guid.Parse(id))
            .ConfigureAwait(false);

        return res.ToProductDto();

    }

    public async Task<List<ProductDto>> GetProductByCategoryAsync(string categoryName)
    {
        var res = await _productRepository
            .FindAsync(p => p.Category == categoryName)
            .ConfigureAwait(false);

        return res.ToProductDtoList();
        
        
    }

    public async Task<List<ProductDto>> GetProductByNameAsync(string name)
    {
        var res = await _productRepository
            .FindAsync(p => p.Name == name)
            .ConfigureAwait(false);

        return res.ToProductDtoList();
    }

    public async Task<ProductDto> CreateProductAsync(ProductRequest product)
    {
        var productDto = product.ToProductDto();
        
        var productId = await _productRepository
            .AddAsync(productDto.ToProductDocument())
            .ConfigureAwait(false);
        
        productDto.Id = productId;

        return productDto;
    }

    public async Task<ProductDto> UpdateProductAsync(ProductRequest product)
    {
        var res = await _productRepository
            .ExistsAsync(product.Id)
            .ConfigureAwait(false);
        if (!res) throw new InvalidOperationException($"Product with id: {product.Id} not found.");
        
        var productDto = product.ToProductDto();
        
        await _productRepository
            .UpdateAsync(productDto.ToProductDocument())
            .ConfigureAwait(false);

        return productDto;
    }

    public async Task<bool> DeleteProductAsync(string id)
    {
        var res = await _productRepository
            .ExistsAsync(Guid.Parse(id))
            .ConfigureAwait(false);

        if (!res)
        {
            return false;
        }

        await _productRepository
            .DeleteAsync(Guid.Parse(id))
            .ConfigureAwait(false);

        return true;
    }
}