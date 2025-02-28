namespace Catalog.API.Dtos;

public class ProductDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Summary { get; set; }
    public string? Description { get; set; }
    public string? ImageFile { get; set; }
    public decimal Price { get; set; }
    public string? Category { get; set; }
    public int Version { get; set; }
}