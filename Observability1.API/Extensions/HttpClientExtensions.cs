using Observability1.API.Services;

namespace Observability1.API.Extensions;

public static class HttpClientExtensions
{
    public static IServiceCollection AddHttpClientConfiguration(this IServiceCollection services)
    {
        services.AddHttpClient<StockService>(x =>
        {
            x.BaseAddress = new Uri("http://localhost:5084");
        });

        return services;
    }
}
