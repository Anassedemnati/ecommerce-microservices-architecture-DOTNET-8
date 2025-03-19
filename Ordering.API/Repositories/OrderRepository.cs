using Microsoft.EntityFrameworkCore;
using Ordering.API.Entities;
using Ordering.API.Persistence;
using Ordering.API.Repositories.Base;

namespace Ordering.API.Repositories;

public class OrderRepository: RepositoryBase<Order> , IOrderRepository
{
    
    public OrderRepository(OrderContext dbContext) : base(dbContext) {}

    public async Task<IEnumerable<Order>> GetOrdersByUserName(string userName)
    {
        var orderList = await _dbContext.Orders
            .Where(o => o.UserName == userName)
            .Include(o => o.Items)
            .ToListAsync();
        return orderList;
    }
}