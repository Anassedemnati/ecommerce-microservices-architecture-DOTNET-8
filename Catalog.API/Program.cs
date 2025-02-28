using Catalog.API.Documents;
using Catalog.API.Repositories;
using Catalog.API.Services;
using Convey;
using Convey.Persistence.MongoDB;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.TryAddScoped<IProductService, ProductService>();
builder.Services.TryAddScoped<IProductRepository, ProductRepository>();
builder.Services.AddConvey()
    .AddMongo()
    .AddMongoRepository<ProductDocument, Guid>("products");

    
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

app.UseAuthorization();

app.MapControllers();

app.Run();
