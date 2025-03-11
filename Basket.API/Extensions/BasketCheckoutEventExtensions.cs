using Basket.API.Entities;
using Basket.API.Events;

namespace Basket.API.Extensions;

public static class BasketCheckoutEventExtensions
{
    public static BasketCheckoutEvent ToBasketCheckoutEvent(this BasketCheckout basketCheckout, ShoppingCart basket)
    {
        return new BasketCheckoutEvent
        {
            UserName = basketCheckout.UserName,
            TotalPrice = basketCheckout.TotalPrice,
            Country = basketCheckout.Country,
            State = basketCheckout.State,
            ZipCode = basketCheckout.ZipCode,
            AddressLine = basketCheckout.AddressLine,
            EmailAddress = basketCheckout.EmailAddress,
            FirstName = basketCheckout.FirstName,
            LastName = basketCheckout.LastName,
            Items = basket.Items.Select(x => new BasketCheckoutEventItem
            {
                ProductId = x.ProductId,
                ProductName = x.ProductName,
                Price = x.Price,
                Quantity = x.Quantity
            }).ToList()
            
            
           
        };
    }
}