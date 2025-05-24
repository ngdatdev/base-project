using BaseApiReference.Abstractions.Caching;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Caching;

/// <summary>
///  Caching Extention
/// </summary>
public static class CachingServivcesExtention
{
    public static IServiceCollection AddCaching(
        this IServiceCollection services,
        IConfiguration configuration,
        CacheType cacheType = CacheType.Memory
    )
    {
        switch (cacheType)
        {
            case CacheType.Memory:
                return services.AddMemoryCaching(configuration);

            case CacheType.Distributed:
                return services.AddDistributedCaching(configuration);

            case CacheType.Hybrid:
                return services.AddHybridCaching(configuration);

            default:
                return services.AddMemoryCaching(configuration);
        }
    }

    public static IServiceCollection AddMemoryCaching(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.Configure<MemoryCacheOptions>(configuration.GetSection("MemoryCache"));

        services.AddMemoryCache(options =>
        {
            var config = configuration.GetSection("MemoryCache").Get<MemoryCacheOptions>();
            if (config?.SizeLimit.HasValue == true)
            {
                options.SizeLimit = config.SizeLimit.Value;
                options.CompactionPercentage = config.CompactionPercentage;
            }
        });

        services.AddSingleton<MemoryCacheHandler>();
        services.AddSingleton<ICacheHandler>(provider => provider.GetService<MemoryCacheHandler>());

        return services;
    }

    public static IServiceCollection AddDistributedCaching(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.Configure<DistributedCacheOption>(configuration.GetSection("DistributedCache"));

        var config = configuration.GetSection("DistributedCache").Get<DistributedCacheOption>();

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = config.ConnectionString;
            options.InstanceName = config.InstanceName;
        });

        services.AddSingleton<DistributedCacheHandler>();
        services.AddSingleton<ICacheHandler>(provider =>
            provider.GetService<DistributedCacheHandler>()
        );

        return services;
    }

    public static IServiceCollection AddHybridCaching(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.Configure<CacheOption>(configuration.GetSection("Cache"));
        services.Configure<MemoryCacheOptions>(configuration.GetSection("MemoryCache"));
        services.Configure<DistributedCacheOption>(configuration.GetSection("DistributedCache"));

        // Add memory cache
        services.AddMemoryCache(options =>
        {
            var config = configuration.GetSection("MemoryCache").Get<MemoryCacheOptions>();
            if (config?.SizeLimit.HasValue == true)
            {
                options.SizeLimit = config.SizeLimit.Value;
                options.CompactionPercentage = config.CompactionPercentage;
            }
        });

        // Add distributed cache
        var distributedConfig = configuration
            .GetSection("DistributedCache")
            .Get<DistributedCacheOption>();
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = distributedConfig.ConnectionString;
            options.InstanceName = distributedConfig.InstanceName;
        });

        services.AddSingleton<MemoryCacheHandler>();
        services.AddSingleton<DistributedCacheHandler>();
        services.AddSingleton<HybridCacheHandler>();
        services.AddSingleton<ICacheHandler>(provider => provider.GetService<HybridCacheHandler>());

        return services;
    }
}
