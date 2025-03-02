using Discount.API.Entities;

namespace Discount.API.Services;

public interface IDiscountService
{
    Task<Coupon?> GetDiscount(string productName);
    Task CreateDiscount(Coupon coupon);
    Task<Coupon?> UpdateDiscount(Coupon coupon);
    Task DeleteDiscount(string productName);
}