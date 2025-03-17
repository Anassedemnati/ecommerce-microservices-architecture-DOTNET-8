using Ordering.API.Entities;
using Ordering.API.Exceptions;
using Ordering.API.Repositories;

namespace Ordering.API.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    
    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    public async Task<IEnumerable<Order>> GetOrdersByUserName(string userName)
    {
        return await _orderRepository.GetOrdersByUserName(userName);
    }

    public async Task<int> CheckoutOrder(Order request)
    {
        var res = await _orderRepository.AddAsync(request);
        return res.Id;
    }

    public async Task UpdateOrder(Order request)
    {
        var order = await _orderRepository.GetByIdAsync(request.Id);
        if (order is null)
        {
            throw new NotFoundException(nameof(Order), request.Id);
        }
        await _orderRepository.UpdateAsync(request);
    }

    public async  Task DeleteOrder(int id)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        if (order is null)
        {
            throw new NotFoundException(nameof(Order), id);
        }
        await _orderRepository.DeleteAsync(order);
    }
}