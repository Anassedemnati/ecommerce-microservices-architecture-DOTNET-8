using Basket.API.Events;
using Basket.API.Middlewares;
using Basket.API.Repositories;
using Basket.API.RestServices;
using Basket.API.Services;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddHttpClient<IDiscountRestService, DiscountRestService>(client =>
    {
        client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("ApiSettings:DiscountUrl")!);
        client.DefaultRequestHeaders.Add("Accept", "application/json");
    }
);

//add MassTransit configuration fot kafka
builder.Services.AddMassTransit(x =>
{
    x.UsingInMemory((context, cfg) =>
    {
        cfg.ConfigureEndpoints(context);
    });

    x.AddRider(rider =>
    {
        rider.AddProducer<BasketCheckoutEvent>(builder.Configuration.GetValue<string>("EventBusSettings:KafkaTopic"));

        rider.UsingKafka((context, k) =>
        {
            k.Host(builder.Configuration.GetValue<string>("EventBusSettings:KafkaUrl"));
        });
    });
});




builder.Services.AddStackExchangeRedisCache(options =>
{
        options.Configuration = builder.Configuration.GetValue<string>("CacheSettings:ConnectionString");
});


builder.Services.AddScoped<IBasketService, BasketService>();
builder.Services.AddScoped<IBasketRepository, BasketRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();