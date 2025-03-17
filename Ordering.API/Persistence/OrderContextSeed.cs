using Ordering.API.Entities;

namespace Ordering.API.Persistence;

public class OrderContextSeed
{
    public static async Task SeedAsync(OrderContext orderContext, ILogger<OrderContextSeed> logger)
    {
        if (!orderContext.Orders.Any())
        {
            orderContext.Orders.AddRange(GetPreconfiguredOrders());
            await orderContext.SaveChangesAsync();
            logger.LogInformation("Seed database associated with context {DbContextName}", typeof(OrderContext).Name);
        }
    }
    private static IEnumerable<Order> GetPreconfiguredOrders()
    {
        return new List<Order>
        {
            new Order() {UserName = "swn",
                FirstName = "Ahmed",
                LastName = "Zakaria",
                EmailAddress = "Ahmed@gmail.com",
                AddressLine = "Lot 1, Jalan 2",
                Country = "Morocco",
                TotalPrice = 350,
                State = "Casablanca",
                ZipCode = "20250",
                Items = new List<OrderItem>
                {
                    new OrderItem {ProductName = "IPhone X", Price = 300, Quantity = 1},
                    new OrderItem {ProductName = "Samsung Galaxy", Price = 50, Quantity = 1}
                }
            }
        };
    }
}