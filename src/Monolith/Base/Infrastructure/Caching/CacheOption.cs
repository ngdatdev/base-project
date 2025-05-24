using System;

namespace Infrastructure.Caching;

/// <summary>
/// Caching options
/// </summary>
public class CacheOption
{
    public TimeSpan DefaultExpiration { get; set; } = TimeSpan.FromMinutes(30);
    public bool EnableLogging { get; set; } = true;
    public string KeyPrefix { get; set; } = "app:cache:";
    public CacheType DefaultCacheType { get; set; } = CacheType.Memory;
}

/// <summary>
/// Caching types
/// </summary>
public enum CacheType
{
    Memory,
    Distributed,
    Hybrid
}
