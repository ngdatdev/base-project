﻿namespace Infrastructure.Caching;

/// summary
///     The RedisOption class is used to hold connectionString redis configuration settings.
/// summary
public sealed class RedisOption
{
    public string ConnectionString { get; set; }
}
