using Basket.API.Models;

namespace Basket.API.RestServices;

public class DiscountRestService : IDiscountRestService
{
    private readonly HttpClient _client;
    
    public DiscountRestService(HttpClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }
    public async Task<CouponModel?> GetDiscount(string? productName)
    {
        var response = await _client.GetAsync($"/api/v1/Discount/{productName}");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<CouponModel>();
        }
        return null;
    }
    
}