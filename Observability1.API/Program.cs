using MassTransit;
using MassTransit.Logging;
using Microsoft.EntityFrameworkCore;
using Observability1.API;
using Observability1.API.Extensions;
using Observability1.API.Models;
using Observability1.API.Services;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);


// MassTransit Configuration
builder.Services.AddMassTransitConfiguration();

// OpenTelemetry Configuration
builder.Services.AddOpenTelemetryConfiguration();

// Redis Configuration
await builder.Services.AddRedisConfiguration(builder.Configuration);

// Entity Framework Core Configuration
builder.Services.AddDatabaseConfiguration(builder.Configuration);

// HTTP Client Configuration
builder.Services.AddHttpClientConfiguration();



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthorization();

app.MapControllers();

app.Run();
