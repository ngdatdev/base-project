using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.Common.Caching;

namespace Infrastructure.Caching;

/// <summary>
///  Caching Extention
/// </summary>
public static class CachingExtention
{
    public static void AddCaching(
        this IServiceCollection services,
        IConfigurationManager configuration
    )
    {
        var option = configuration
            .GetRequiredSection(key: "Cache")
            .GetRequiredSection(key: "Redis")
            .Get<RedisOption>();

        services.AddStackExchangeRedisCache(setupAction: config =>
        {
            config.Configuration = option.ConnectionString;
        });

        services.AddScoped<ICacheHandler, RedisCacheHandler>();
    }
}
