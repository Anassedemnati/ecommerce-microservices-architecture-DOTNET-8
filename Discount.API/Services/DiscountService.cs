using Discount.API.Entities;
using Discount.API.Repositories;

namespace Discount.API.Services;

public class DiscountService : IDiscountService
{
    private readonly IDiscountRepository _repository;
    
    public DiscountService(IDiscountRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }


    public async Task<Coupon?> GetDiscount(string productName)
    {
        return await _repository.GetDiscount(productName);
    }

    public async Task CreateDiscount(Coupon coupon)
    { 
        var result = await _repository.CreateDiscount(coupon);
        if (!result)
            throw new Exception("Failed to create discount");
    }

    public async Task<Coupon?> UpdateDiscount(Coupon coupon)
    {
        var result = await _repository.UpdateDiscount(coupon);
        if (!result)
            throw new Exception("Failed to update discount");
        
        return coupon;
    }

    public Task DeleteDiscount(string productName)
    {
        return _repository.DeleteDiscount(productName);
    }
}