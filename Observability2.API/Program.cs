using MassTransit;
using MassTransit.Logging;
using Observability2.API.Consumers;
using Observability2.API.Extensions;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

// MassTransit Configuration
builder.Services.AddMassTransitConfiguration();

// OpenTelemetry Configuration
builder.Services.AddOpenTelemetryConfiguration();



builder.Services.AddControllers();
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
