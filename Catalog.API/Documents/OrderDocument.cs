using Convey.Types;

namespace Catalog.API.Documents;

public class OrderDocument : IIdentifiable<Guid>
{
    public Guid Id { get; set; }
    public Guid BuyerId { get; set; }
    public AddressDocument? ShippingAddress { get; set; }
    public OrderStatus? Status { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime CreatedAt { get; set; }
    public IEnumerable<OrderItemDocument>? Items { get; set; }
    public int Version { get; set; }

}
