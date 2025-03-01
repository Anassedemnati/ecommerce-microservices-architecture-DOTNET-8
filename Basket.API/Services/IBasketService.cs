using Basket.API.Entities;

namespace Basket.API.Services;

public interface IBasketService
{
    Task<ShoppingCart?> GetBasket(string? userName);
    Task<ShoppingCart?> UpdateBasket(ShoppingCart basket);
    Task DeleteBasket(string? userName);
}