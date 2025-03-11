using System.Net;
using Basket.API.Entities;
using Basket.API.Extensions;
using Basket.API.Services;
using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Basket.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class BasketController : ControllerBase
{
    private readonly ILogger<BasketController> _logger;
    private readonly IBasketService _basketService;
    private readonly IProducer<Null, string> _kafkaProducer;
    private readonly string _kafkaTopic;
    
    public BasketController(ILogger<BasketController> logger, IBasketService basketService, IProducer<Null, string> kafkaProducer, IConfiguration configuration)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _basketService = basketService ?? throw new ArgumentNullException(nameof(basketService));
        _kafkaProducer = kafkaProducer ?? throw new ArgumentNullException(nameof(kafkaProducer));
        _kafkaTopic = configuration.GetValue<string>("EventBusSettings:KafkaTopic") ?? throw new ArgumentNullException(nameof(kafkaProducer)); 
    }


    
    [HttpGet("{userName}", Name = "GetBasket")]
    public async Task<ActionResult<ShoppingCart>> GetBasket(string? userName)
    {
        var basket = await _basketService.GetBasket(userName);
        return Ok(basket ?? new ShoppingCart(userName));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket)
    {
        return Ok(await _basketService.UpdateBasket(basket));
    }

    [HttpDelete("{userName}", Name = "DeleteBasket")]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> DeleteBasket(string? userName)
    {
        await _basketService.DeleteBasket(userName);
        return Ok();
    }
    
    [Route("[action]")]
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Accepted)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
    {
        try
        {
            // get existing basket with total price            
            // Set TotalPrice on basketCheckout eventMessage
            // send checkout event to rabbitmq
            // remove the basket

            // get existing basket with total price
            var basket = await _basketService.GetBasket(basketCheckout.UserName);
            if (basket == null)
            {
                return BadRequest();
            }

            // send checkout event to Kafka
            var eventMessage = basketCheckout.ToBasketCheckoutEvent(basket);
            
            var message = JsonConvert.SerializeObject(eventMessage);
            _logger.LogInformation("Publishing event to Kafka");

            await _kafkaProducer.ProduceAsync(_kafkaTopic, new Message<Null, string> { Value = message });
            _logger.LogInformation("Event published to Kafka");
            
            // remove the basket
            await _basketService.DeleteBasket(basket.UserName);
            _logger.LogInformation($"Basket removed for {basket.UserName}"); 

            return Accepted();

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }
    
    
    
    
}