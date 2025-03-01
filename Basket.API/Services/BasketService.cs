using Basket.API.Entities;
using Basket.API.Repositories;

namespace Basket.API.Services;

public class BasketService : IBasketService
{
    private readonly IBasketRepository _repository;
    
    public BasketService(IBasketRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }
    
    public Task<ShoppingCart?> GetBasket(string? userName)
    {
        return _repository.GetBasket(userName);
    }

    public Task<ShoppingCart?> UpdateBasket(ShoppingCart basket)
    {
        return _repository.UpdateBasket(basket);
    }

    public Task DeleteBasket(string? userName)
    {
        return _repository.DeleteBasket(userName);
    }
}