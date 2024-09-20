using MassTransit.Logging;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Observability1.API.Extensions;

public static class OpenTelemetryExtensions
{
    public static IServiceCollection AddOpenTelemetryConfiguration(this IServiceCollection services)
    {
        services.AddOpenTelemetry().WithTracing(x =>
        {
            x.AddSource("Observability.API.ActivitySource");
            x.AddSource(DiagnosticHeaders.DefaultListenerName);
            x.ConfigureResource(rb => rb.AddService("Observability.API", serviceVersion: "1.0"));

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
            x.AddOtlpExporter();
        }).WithLogging(logging =>
        {
            logging.AddOtlpExporter();
            logging.ConfigureResource(rb => rb.AddService("Observability.API", serviceVersion: "1.0"));
        }).WithMetrics(metrics =>
        {
            metrics.AddProcessInstrumentation();
            metrics.AddRuntimeInstrumentation();
            metrics.ConfigureResource(rb => rb.AddService("Observability.API", serviceVersion: "1.0"));
            metrics.AddOtlpExporter();
        });

        return services;
    }
}
