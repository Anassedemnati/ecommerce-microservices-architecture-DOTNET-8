using Ordering.API.Entities;

namespace Ordering.API.Services;

public interface IOrderService
{
    Task<IEnumerable<Order>> GetOrdersByUserName(string userName);
    Task<int> CheckoutOrder(Order request);
    Task UpdateOrder(Order request);
    Task DeleteOrder(int id);
}