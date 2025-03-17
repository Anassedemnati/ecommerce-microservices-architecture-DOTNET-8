using Confluent.Kafka;
using Newtonsoft.Json;
using Ordering.API.Events;
using Ordering.API.Extensions;
using Ordering.API.Services;

namespace Ordering.API.EventConsumer;

public class BasketCheckoutConsumer : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IConsumer<Null, string> _consumer;
    private readonly ILogger<BasketCheckoutConsumer> _logger;

    public BasketCheckoutConsumer(
        IServiceScopeFactory serviceScopeFactory,
        ILogger<BasketCheckoutConsumer> logger,
        IConfiguration configuration)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;

        var consumerConfig = new ConsumerConfig
        {
            GroupId = configuration["Kafka:GroupId"],
            BootstrapServers = configuration["Kafka:BootstrapServers"],
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        _consumer = new ConsumerBuilder<Null, string>(consumerConfig).Build();
        _consumer.Subscribe(configuration["Kafka:Topic"]);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var consumeResult = _consumer.Consume(stoppingToken);
                var basketCheckoutEvent =
                    JsonConvert.DeserializeObject<BasketCheckoutEvent>(consumeResult.Message.Value);

                if (basketCheckoutEvent is null)
                {
                    _logger.LogWarning("Invalid message received");
                    continue;
                }

                using var scope = _serviceScopeFactory.CreateScope();
                var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();

                var order = basketCheckoutEvent.ToOrder();
                await orderService.CheckoutOrder(order);

                _logger.LogInformation($"Processed message: {consumeResult.Message.Value}");
            }
            catch (ConsumeException ex)
            {
                _logger.LogError($"Error consuming message: {ex.Message}");
            }
        }
    }
}
