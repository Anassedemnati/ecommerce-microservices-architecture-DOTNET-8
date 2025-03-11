using System.Text.Json;
using Discount.API.Extension;
using Discount.API.Middlewares;
using Discount.API.Repositories;
using Discount.API.Services;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<IDiscountService, DiscountService>();
builder.Services.AddSingleton<IDiscountRepository, DiscountRepository>();

builder.Services.AddHealthChecks()
    .AddNpgSql(
        connectionString: builder.Configuration.GetSection("DatabaseSettings:ConnectionString").Value!,
        name: "postgres",
        failureStatus: HealthStatus.Unhealthy,
        tags: new[] { "db", "sql", "postgres" });
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.MapHealthChecks("/health", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = async (context, report) =>
    {
        var result = JsonSerializer.Serialize(new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                description = e.Value.Description
            }),
            duration = report.TotalDuration.TotalMilliseconds
        });

        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(result);
    }
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//exception handling middleware


//databse migration on startup

app.UseAuthorization();
app.MigrateDatabase<Program>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();