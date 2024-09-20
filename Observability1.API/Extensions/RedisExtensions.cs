using StackExchange.Redis;

namespace Observability1.API.Extensions;

public static class RedisExtensions
{
    public static async Task<IServiceCollection> AddRedisConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        IConnectionMultiplexer redisConnectionMultiplexer = await ConnectionMultiplexer.ConnectAsync("localhost");
        services.AddSingleton(redisConnectionMultiplexer);
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis");
            options.InstanceName = "SampleInstance";
            options.ConnectionMultiplexerFactory = () => Task.FromResult(redisConnectionMultiplexer);
        });

        return services;
    }
}
