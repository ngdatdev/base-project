using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseApiReference.Abstractions.Caching;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Caching;

/// <summary>
/// Implementation of cache handler using hybrid cache as storage.
/// </summary>
public class HybridCacheHandler : ICacheHandler
{
    private readonly ICacheHandler _memoryCache;
    private readonly ICacheHandler _distributedCache;
    private readonly ILogger<HybridCacheHandler> _logger;
    private readonly CacheOption _options;

    public HybridCacheHandler(
        MemoryCacheHandler memoryCache,
        DistributedCacheHandler distributedCache,
        ILogger<HybridCacheHandler> logger,
        IOptions<CacheOption> options
    )
    {
        _memoryCache = memoryCache;
        _distributedCache = distributedCache;
        _logger = logger;
        _options = options.Value;
    }

    public T Get<T>(string key)
    {
        var cached = _memoryCache.Get<T>(key);
        if (cached != null)
            return cached;

        cached = _distributedCache.Get<T>(key);
        if (cached != null)
        {
            _memoryCache.Set(key, cached, TimeSpan.FromMinutes(5));
        }

        return cached;
    }

    public bool Set<T>(string key, T value, TimeSpan? expiration = null)
    {
        var memoryResult = _memoryCache.Set(key, value, expiration);
        var distributedResult = _distributedCache.Set(key, value, expiration);
        return memoryResult && distributedResult;
    }

    public bool Remove(string key)
    {
        var memoryResult = _memoryCache.Remove(key);
        var distributedResult = _distributedCache.Remove(key);
        return memoryResult && distributedResult;
    }

    public bool Exists(string key)
    {
        return _memoryCache.Exists(key) || _distributedCache.Exists(key);
    }

    public void Clear()
    {
        _memoryCache.Clear();
        _distributedCache.Clear();
    }

    public async Task<T> GetAsync<T>(string key)
    {
        var cached = await _memoryCache.GetAsync<T>(key);
        if (cached != null)
            return cached;

        cached = await _distributedCache.GetAsync<T>(key);
        if (cached != null)
        {
            await _memoryCache.SetAsync(key, cached, TimeSpan.FromMinutes(5));
        }

        return cached;
    }

    public async Task<bool> SetAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        var memoryTask = _memoryCache.SetAsync(key, value, expiration);
        var distributedTask = _distributedCache.SetAsync(key, value, expiration);

        var results = await Task.WhenAll(memoryTask, distributedTask);
        return results[0] && results[1];
    }

    public async Task<bool> RemoveAsync(string key)
    {
        var memoryTask = _memoryCache.RemoveAsync(key);
        var distributedTask = _distributedCache.RemoveAsync(key);

        var results = await Task.WhenAll(memoryTask, distributedTask);
        return results[0] && results[1];
    }

    public async Task<bool> ExistsAsync(string key)
    {
        var memoryExists = await _memoryCache.ExistsAsync(key);
        if (memoryExists)
            return true;

        return await _distributedCache.ExistsAsync(key);
    }

    public async Task ClearAsync()
    {
        await Task.WhenAll(_memoryCache.ClearAsync(), _distributedCache.ClearAsync());
    }

    public async Task<T> GetOrSetAsync<T>(
        string key,
        Func<Task<T>> factory,
        TimeSpan? expiration = null
    )
    {
        var cached = await GetAsync<T>(key);
        if (cached != null)
            return cached;

        var value = await factory();
        if (value != null)
            await SetAsync(key, value, expiration);
        return value;
    }

    public T GetOrSet<T>(string key, Func<T> factory, TimeSpan? expiration = null)
    {
        var cached = Get<T>(key);
        if (cached != null)
            return cached;

        var value = factory();
        if (value != null)
            Set(key, value, expiration);
        return value;
    }

    public async Task RefreshAsync(string key)
    {
        await _distributedCache.RefreshAsync(key);
    }

    public async Task<bool> SetMultipleAsync<T>(
        Dictionary<string, T> items,
        TimeSpan? expiration = null
    )
    {
        var memoryTask = _memoryCache.SetMultipleAsync(items, expiration);
        var distributedTask = _distributedCache.SetMultipleAsync(items, expiration);

        var results = await Task.WhenAll(memoryTask, distributedTask);
        return results[0] && results[1];
    }

    public async Task<Dictionary<string, T>> GetMultipleAsync<T>(IEnumerable<string> keys)
    {
        var memoryResult = await _memoryCache.GetMultipleAsync<T>(keys);
        var missingKeys = new List<string>();

        foreach (var key in keys)
        {
            if (!memoryResult.ContainsKey(key))
                missingKeys.Add(key);
        }

        if (missingKeys.Count > 0)
        {
            var distributedResult = await _distributedCache.GetMultipleAsync<T>(missingKeys);

            foreach (var item in distributedResult)
            {
                await _memoryCache.SetAsync(item.Key, item.Value, TimeSpan.FromMinutes(5));
                memoryResult[item.Key] = item.Value;
            }
        }

        return memoryResult;
    }
}
