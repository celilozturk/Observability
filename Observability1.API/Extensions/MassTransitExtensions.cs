using MassTransit;

namespace Observability1.API.Extensions;

public static class MassTransitExtensions
{
    public static IServiceCollection AddMassTransitConfiguration(this IServiceCollection services)
    {
        services.AddMassTransit(x =>
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

        return services;
    }
}
