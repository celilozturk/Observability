using MassTransit;
using MassTransit.Logging;
using Microsoft.EntityFrameworkCore;
using Observability1.API;
using Observability1.API.Models;
using Observability1.API.Services;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

//masstransit
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
    });
});

 //Open Telemetry
//Datalari toplama islemi opentelemetry burada
builder.Services.AddOpenTelemetry().WithTracing(x =>
{
    x.AddSource("Observability.API.ActivitySource");
    //MassTransit
    x.AddSource(DiagnosticHeaders.DefaultListenerName);
    x.ConfigureResource(rb => rb.AddService("Observability.API", serviceVersion: "1.0")); //resourse builder rb

    x.AddAspNetCoreInstrumentation(x =>
    {
        x.Filter = (httpContext) => httpContext.Request.Path.Value.Contains("api");
        
    });
    x.AddEntityFrameworkCoreInstrumentation(x =>
    {
        x.SetDbStatementForText = true;
        x.SetDbStatementForStoredProcedure = true;
    });
    x.AddHttpClientInstrumentation();
    x.AddRedisInstrumentation(x => { x.SetVerboseDatabaseStatements = true; });

    x.AddConsoleExporter();
    x.AddOtlpExporter(); //native 4317 gRPC uzerinde gonderir(Jeager dan gormek icin)
}).WithLogging(logging =>
{
   logging.AddOtlpExporter();
   logging.ConfigureResource(rb => rb.AddService("Observability.API", serviceVersion: "1.0")); //resourse builder rb

}).WithMetrics(metrics =>
{
    metrics.AddProcessInstrumentation();
    metrics.AddRuntimeInstrumentation();
    metrics.ConfigureResource(rb => rb.AddService("Observability.API", serviceVersion: "1.0")); //resourse builder rb
    metrics.AddOtlpExporter();
});

builder.Services.AddHttpClient<StockService>(x =>
{
    x.BaseAddress = new Uri("http://localhost:5084");
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});

IConnectionMultiplexer redisConnectionMultiplexer = await ConnectionMultiplexer.ConnectAsync("localhost");
builder.Services.AddSingleton(redisConnectionMultiplexer);
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "SampleInstance";

    options.ConnectionMultiplexerFactory = () => Task.FromResult(redisConnectionMultiplexer);
    
});
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
