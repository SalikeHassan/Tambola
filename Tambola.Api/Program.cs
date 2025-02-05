using Microsoft.AspNetCore.Mvc;
using Tambola.Api.src.Application.Strategies;
using Tambola.Api.src.Application.Strategies.Factory;
using Tambola.Api.src.Application.Validators;
using Tambola.Api.src.Domain;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<IGameStrategy,TopLineStrategy>();
builder.Services.AddTransient<IGameStrategy,MiddleLineStrategy>();
builder.Services.AddTransient<IGameStrategy, BottomLineStrategy>();
builder.Services.AddTransient<IGameStrategy,FullHouseStrategy>();
builder.Services.AddTransient<IGameStrategy,EarlyFiveStrategy>();

// Register the factory with the strategies injected
builder.Services.AddSingleton<IGameStrategyFactory,GameStrategyFactory>();

builder.Services.AddScoped<IClaimValidator, ClaimValidator>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.MapControllers();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}