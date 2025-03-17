using Ordering.API.Entities.Common;

namespace Ordering.API.Entities;

public class OrderItem : EntityBase
{
    public string? ProductId { get; set; }
    public string? ProductName { get; set; } 
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}