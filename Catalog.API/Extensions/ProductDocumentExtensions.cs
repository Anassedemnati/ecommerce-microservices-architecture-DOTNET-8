using Catalog.API.Documents;
using Catalog.API.Dtos;
using Catalog.API.Requests;

namespace Catalog.API.Extensions;

public static class ProductDocumentExtensions
{
    public static ProductDto ToProductDto(this ProductDocument productDocument)
    {
        return new ProductDto
        {
            Id = productDocument.Id,
            Name = productDocument.Name,
            Summary = productDocument.Summary,
            Description = productDocument.Description,
            ImageFile = productDocument.ImageFile,
            Price = productDocument.Price,
            Category = productDocument.Category,
            Version = productDocument.Version
        };
    }
    public static List<ProductDto> ToProductDtoList(this IEnumerable<ProductDocument> productDocuments)
    {
        return productDocuments.Select(pd => pd.ToProductDto()).ToList();
    }
    
    
    public static ProductDto ToProductDto(this ProductRequest productRequest)
    {
        return new ProductDto
        {
            Id = productRequest.Id,
            Name = productRequest.Name,
            Summary = productRequest.Summary,
            Description = productRequest.Description,
            ImageFile = productRequest.ImageFile,
            Price = productRequest.Price,
            Category = productRequest.Category
        };
    }
    
    public static ProductDocument ToProductDocument(this ProductDto productDto)
    {
        return new ProductDocument
        {
            Id = productDto.Id,
            Name = productDto.Name,
            Summary = productDto.Summary,
            Description = productDto.Description,
            ImageFile = productDto.ImageFile,
            Price = productDto.Price,
            Category = productDto.Category,
            Version = productDto.Version
        };
    }
    
}
    
    

    
    
    