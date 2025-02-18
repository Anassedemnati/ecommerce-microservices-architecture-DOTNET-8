using Convey.Types;

namespace Catalog.API.Documents;

public class OrderItemDocument : IIdentifiable<Guid>
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Price { get; set; }
}