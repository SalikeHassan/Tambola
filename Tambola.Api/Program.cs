using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tambola.Api.src.Application.Behaviors;
using Tambola.Api.src.Application.Services;
using Tambola.Api.src.Application.Strategies;
using Tambola.Api.src.Application.Strategies.Factory;
using Tambola.Api.src.Application.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<IGameStrategy,TopLineStrategy>();
builder.Services.AddTransient<IGameStrategy,MiddleLineStrategy>();
builder.Services.AddTransient<IGameStrategy, BottomLineStrategy>();
builder.Services.AddTransient<IGameStrategy,FullHouseStrategy>();
builder.Services.AddTransient<IGameStrategy,EarlyFiveStrategy>();

builder.Services.AddSingleton<IGameStrategyFactory,GameStrategyFactory>();

builder.Services.AddScoped<IClaimTrackerService,ClaimTrackerService>();
builder.Services.AddScoped<IClaimValidationService,ClaimValidationService>();
builder.Services.AddScoped<IClaimValidator, ClaimValidator>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TicketValidationBehavior<,>));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(DuplicateClaimBehavior<,>));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(GameValidationBehavior<,>));

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

app.MapControllers();

app.Run();