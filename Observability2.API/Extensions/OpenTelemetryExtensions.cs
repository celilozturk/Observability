using MassTransit.Logging;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Observability2.API.Extensions;

public static class OpenTelemetryExtensions
{
    public static IServiceCollection AddOpenTelemetryConfiguration(this IServiceCollection services)
    {
        services.AddOpenTelemetry().WithTracing(tracing =>
        {
            tracing.AddSource(DiagnosticHeaders.DefaultListenerName);
            tracing.AddSource("Observability2.API.ActivitySource");
            tracing.ConfigureResource(rb => rb.AddService("Observability2.API", serviceVersion: "1.0"));
            tracing.AddOtlpExporter();
            tracing.AddAspNetCoreInstrumentation();
        })
        .WithLogging(logging =>
        {
            logging.AddOtlpExporter();
            logging.ConfigureResource(rb => rb.AddService("Observability2.API", serviceVersion: "1.0"));
        })
        .WithMetrics(metrics =>
        {
            metrics.AddProcessInstrumentation();
            metrics.AddRuntimeInstrumentation();
            metrics.ConfigureResource(rb => rb.AddService("Observability2.API", serviceVersion: "1.0"));
            metrics.AddOtlpExporter();
        });

        return services;
    }
}
