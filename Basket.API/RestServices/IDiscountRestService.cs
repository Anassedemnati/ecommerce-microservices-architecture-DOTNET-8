using Basket.API.Models;

namespace Basket.API.RestServices;

public interface IDiscountRestService
{
    public Task<CouponModel?> GetDiscount(string? productName);

}