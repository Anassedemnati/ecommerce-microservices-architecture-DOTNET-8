using Catalog.API.Requests;
using Catalog.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CatalogController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ILogger<CatalogController> _logger;

    public CatalogController(IProductService productService, ILogger<CatalogController> logger)
    {
        _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var products = await _productService.GetProductsAsync();
        return Ok(products); 
    }

    [HttpGet("{id:length(24)}", Name = "GetProduct")]
    public async Task<IActionResult> GetProductById(string id)
    {
        var product = await _productService.GetProductAsync(id);
        if (product is null)
        {
            _logger.LogError($"Product with id: {id}, not found.");
            return NotFound(); 
        }
        return Ok(product); 
    }

    [HttpGet("category/{categoryName}", Name = "GetProductByCategory")]
    public async Task<IActionResult> GetProductByCategory(string categoryName)
    {
        var products = await _productService.GetProductByCategoryAsync(categoryName);
        if (!products.Any())
        {
            _logger.LogInformation($"No products found for category: {categoryName}.");
            return NotFound(); 
        }
        return Ok(products); 
    }

    [HttpGet("name/{name}", Name = "GetProductByName")]
    public async Task<IActionResult> GetProductByName(string name)
    {
        var products = await _productService.GetProductByNameAsync(name);
        if (!products.Any())
        {
            _logger.LogInformation($"No products found with name: {name}.");
            return NotFound(); 
        }
        return Ok(products); 
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] ProductRequest product)
    {
        //TODO add validation for product request by using fluent validation

        var createdProduct = await _productService.CreateProductAsync(product);
        return CreatedAtAction("CreateProduct", new { id = createdProduct.Id }, createdProduct);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> UpdateProduct(string id, [FromBody] ProductRequest product)
    {
        try
        {
            //TODO add validation for product request by using fluent validation
            var updatedProduct = await _productService.UpdateProductAsync(product);
            if (updatedProduct is null)
            {
                _logger.LogError($"Product with id: {id}, not found.");
                return NotFound();
            }

            return Ok(updatedProduct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error while updating product with id: {id}.");
            return BadRequest();
        }

    }

    [HttpDelete("{id:length(24)}", Name = "DeleteProduct")]
    public async Task<IActionResult> DeleteProductById(string id)
    {
        var isDeleted = await _productService.DeleteProductAsync(id);
        if (!isDeleted)
        {
            _logger.LogError($"Product with id: {id}, not found.");
            return NotFound(); // 404 Not Found if the product doesn't exist
        }

        return NoContent(); // 204 No Content if the product is successfully deleted
    }
}