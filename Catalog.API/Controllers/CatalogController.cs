using Catalog.API.Documents;
using Catalog.API.Repositories;
using Catalog.API.Requests;
using Catalog.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers;
[ApiController]
[Route("api/v1/Catalog")]
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
        var products = await _productService.GetProducts();
        return Ok(products);
    }

        
    [HttpGet("{id:length(24)}", Name = "GetProduct")]
    public async Task<IActionResult> GetProductById(string id)
    {
        var product = await _productService.GetProduct(id);
        if (product == null)
        {
            _logger.LogError($"Product with id: {id}, not found.");
            return NotFound();
        }
        return Ok(product);
    }
    
    
        
    [HttpGet("[action]/{categoryName}", Name = "GetProductByCategory")]
    public async Task<IActionResult> GetProductByCategory(string categoryName)
    {
        var products = await _productService.GetProductByCategory(categoryName);
        return Ok(products);
    }
    [HttpGet("[action]/{name}", Name = "GetProductByName")]
    public async Task<IActionResult> GetProductByName(string name)
    {
        var products = await _productService.GetProductByName(name);
        return Ok(products);
    }
    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] ProductRequest product)
    {
        var res = await _productService.CreateProduct(product);
        return CreatedAtRoute("GetProduct", new { id = res }, product);
    }
    [HttpPut]
    public async Task<IActionResult> UpdateProduct([FromBody] ProductRequest product)
    {
        return Ok(await _productService.UpdateProduct(product));
    }
    [HttpDelete("{id:length(24)}", Name = "DeleteProduct")]
    public async Task<IActionResult> DeleteProductById(string id)
    {
        return Ok(await _productService.DeleteProduct(id));
    }
   
}