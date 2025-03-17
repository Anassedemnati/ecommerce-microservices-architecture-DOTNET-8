using Ordering.API.Entities;
using Ordering.API.Repositories.Base;

namespace Ordering.API.Repositories;

public interface IOrderRepository: IAsyncRepository<Order>
{
    Task<IEnumerable<Order>> GetOrdersByUserName(string userName);
}