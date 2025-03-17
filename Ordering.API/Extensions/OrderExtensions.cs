using Ordering.API.Entities;
using Ordering.API.Events;

namespace Ordering.API.Extensions;

public static class OrderExtensions
{
    public static Order ToOrder(this BasketCheckoutEvent basketCheckoutEvent)
    {
        var order = new Order
        {
            UserName = basketCheckoutEvent.UserName,
            FirstName = basketCheckoutEvent.FirstName,
            LastName = basketCheckoutEvent.LastName,
            EmailAddress = basketCheckoutEvent.EmailAddress,
            AddressLine = basketCheckoutEvent.AddressLine,
            Country = basketCheckoutEvent.Country,
            State = basketCheckoutEvent.State,
            ZipCode = basketCheckoutEvent.ZipCode,
            TotalPrice = basketCheckoutEvent.TotalPrice
        };

        foreach (var item in basketCheckoutEvent.Items)
        {
            order.Items.Add(new OrderItem
            {
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                Price = item.Price,
                Quantity = item.Quantity
            });
        }

        return order;
    }
}