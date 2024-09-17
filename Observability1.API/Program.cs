using Microsoft.EntityFrameworkCore;
using Observability1.API.Models;
using OpenTelemetry.Trace;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

//Datalari toplama islemi opentelemetry burada
builder.Services.AddOpenTelemetry().WithTracing(x =>
{
    x.AddSource("Observability.API.ActivitySource");

    x.AddAspNetCoreInstrumentation();
    x.AddEntityFrameworkCoreInstrumentation(x =>
    {
        x.SetDbStatementForText = true;
        x.SetDbStatementForStoredProcedure = true;
    });
    x.AddHttpClientInstrumentation();
    x.AddRedisInstrumentation(x => { x.SetVerboseDatabaseStatements = true; });

    x.AddConsoleExporter();
    x.AddOtlpExporter(); //native 4317 gRPC uzerinde gonderir(Jeager dan gormek icin)
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
