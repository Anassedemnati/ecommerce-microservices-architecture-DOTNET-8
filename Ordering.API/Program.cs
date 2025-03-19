using KafkaFlow;
using Microsoft.EntityFrameworkCore;
using Ordering.API.EventConsumer;
using Ordering.API.Persistence;
using Ordering.API.Repositories;
using Ordering.API.Repositories.Base;
using Ordering.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<OrderContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();


builder.Services.AddKafka(kafka => kafka
    .UseConsoleLog()
    .AddCluster(cluster => cluster
        .WithBrokers(new []
        {
            builder.Configuration["KafkaSettings:Broker"]
        })
        .AddConsumer(consumer => consumer
            .Topic(builder.Configuration["KafkaSettings:TopicName"])
            .WithGroupId("order-api")
            .WithBufferSize(100)
            .WithWorkersCount(3)
            .WithAutoOffsetReset(AutoOffsetReset.Earliest)
            .AddMiddlewares(middlewares => 
                    middlewares.Add<BasketCheckoutMiddleware>(MiddlewareLifetime.Transient)
                )
            )
        )
);

// Add services to the container.


//builder.Services.AddHostedService<BasketCheckoutConsumer>();

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

var bus = app.Services.CreateKafkaBus();
await bus.StartAsync();

DatabaseInitializer.Initialize(app.Services);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();