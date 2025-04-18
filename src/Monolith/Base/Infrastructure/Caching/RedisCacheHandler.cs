﻿using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using BaseApiReference.Abstractions.Caching;
using Microsoft.Extensions.Caching.Distributed;
using PenomyAPI.App.Common.Caching;

namespace Infrastructure.Caching;

/// <summary>
///     Implementation of cache handler using redis as storage.
/// </summary>
public sealed class RedisCacheHandler : ICacheHandler
{
    private readonly IDistributedCache _distributedCache;

    public RedisCacheHandler(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public async Task<AppCacheModel<TSource>> GetAsync<TSource>(
        string key,
        CancellationToken cancellationToken
    )
    {
        var cachedValue = await _distributedCache.GetStringAsync(
            key: key,
            token: cancellationToken
        );

        if (string.IsNullOrWhiteSpace(value: cachedValue))
        {
            return AppCacheModel<TSource>.NotFound;
        }

        return new() { Value = JsonSerializer.Deserialize<TSource>(json: cachedValue) };
    }

    public Task<TSource> GetOrSetAsync<TSource>(
        string key,
        Func<CancellationToken, Task<TSource>> value,
        AppCacheOption cacheOption,
        CancellationToken cancellationToken
    )
    {
        throw new NotImplementedException();
    }

    public Task RemoveAsync(string key, CancellationToken cancellationToken)
    {
        return _distributedCache.RemoveAsync(key: key, token: cancellationToken);
    }

    public Task SetAsync<TSource>(
        string key,
        TSource value,
        DistributedCacheEntryOptions distributedCacheEntryOptions,
        CancellationToken cancellationToken
    )
    {
        return _distributedCache.SetStringAsync(
            key: key,
            value: JsonSerializer.Serialize(value: value),
            options: distributedCacheEntryOptions,
            token: cancellationToken
        );
    }
}
