using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOpenTelemetry().WithTracing(tracing =>
{
    tracing.AddSource("Observability2.API.ActivitySource");
    tracing.ConfigureResource(rb => rb.AddService("Observability2.API", serviceVersion: "1.0")); //resourse builder rb

    tracing.AddOtlpExporter();
    tracing.AddAspNetCoreInstrumentation();
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
