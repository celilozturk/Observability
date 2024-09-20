using MassTransit;
using Observability2.API.Consumers;

namespace Observability2.API.Extensions;

public static class MassTransitExtensions
{
    public static IServiceCollection AddMassTransitConfiguration(this IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<ProductAddedEventConsumer>();
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
                cfg.ReceiveEndpoint("observability2-prodct.created.event-queue", e =>
                {
                    e.ConfigureConsumer<ProductAddedEventConsumer>(context);
                });
            });
        });

        return services;
    }
}
