using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BaseApiReference.Abstractions.Caching;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Caching;

/// <summary>
/// Implementation of cache handler using distributed cache as storage.
/// </summary>
public class DistributedCacheHandler : ICacheHandler
{
    private readonly IDistributedCache _distributedCache;
    private readonly ILogger<DistributedCacheHandler> _logger;
    private readonly DistributedCacheOption _options;

    public DistributedCacheHandler(
        IDistributedCache distributedCache,
        ILogger<DistributedCacheHandler> logger,
        IOptions<DistributedCacheOption> options
    )
    {
        _distributedCache = distributedCache;
        _logger = logger;
        _options = options.Value;
    }

    public T Get<T>(string key)
    {
        try
        {
            var fullKey = GetFullKey(key);
            var cachedValue = _distributedCache.GetString(fullKey);

            if (cachedValue == null)
            {
                if (_options.EnableLogging)
                    _logger.LogDebug("Cache miss for key: {Key}", key);
                return default(T);
            }

            var result = JsonSerializer.Deserialize<T>(cachedValue);

            if (_options.EnableLogging)
                _logger.LogDebug("Cache hit for key: {Key}", key);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting cache key: {Key}", key);
            return default(T);
        }
    }

    public bool Set<T>(string key, T value, TimeSpan? expiration = null)
    {
        try
        {
            var fullKey = GetFullKey(key);
            var serializedValue = JsonSerializer.Serialize(value);
            var exp = expiration ?? _options.DefaultExpiration;

            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = exp
            };

            _distributedCache.SetString(fullKey, serializedValue, options);

            if (_options.EnableLogging)
                _logger.LogDebug("Set cache key: {Key} with expiration: {Expiration}", key, exp);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error setting cache key: {Key}", key);
            return false;
        }
    }

    public bool Remove(string key)
    {
        try
        {
            var fullKey = GetFullKey(key);
            _distributedCache.Remove(fullKey);

            if (_options.EnableLogging)
                _logger.LogDebug("Removed cache key: {Key}", key);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing cache key: {Key}", key);
            return false;
        }
    }

    public bool Exists(string key)
    {
        var fullKey = GetFullKey(key);
        var value = _distributedCache.GetString(fullKey);
        return value != null;
    }

    public void Clear()
    {
        // Note: IDistributedCache doesn't have a clear method
        // This would need Redis-specific implementation
        _logger.LogWarning("Clear operation not supported for distributed cache");
    }

    public async Task<T> GetAsync<T>(string key)
    {
        try
        {
            var fullKey = GetFullKey(key);
            var cachedValue = await _distributedCache.GetStringAsync(fullKey);

            if (cachedValue == null)
            {
                if (_options.EnableLogging)
                    _logger.LogDebug("Cache miss for key: {Key}", key);
                return default(T);
            }

            var result = JsonSerializer.Deserialize<T>(cachedValue);

            if (_options.EnableLogging)
                _logger.LogDebug("Cache hit for key: {Key}", key);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting cache key: {Key}", key);
            return default(T);
        }
    }

    public async Task<bool> SetAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        try
        {
            var fullKey = GetFullKey(key);
            var serializedValue = JsonSerializer.Serialize(value);
            var exp = expiration ?? _options.DefaultExpiration;

            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = exp
            };

            await _distributedCache.SetStringAsync(fullKey, serializedValue, options);

            if (_options.EnableLogging)
                _logger.LogDebug("Set cache key: {Key} with expiration: {Expiration}", key, exp);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error setting cache key: {Key}", key);
            return false;
        }
    }

    public async Task<bool> RemoveAsync(string key)
    {
        try
        {
            var fullKey = GetFullKey(key);
            await _distributedCache.RemoveAsync(fullKey);

            if (_options.EnableLogging)
                _logger.LogDebug("Removed cache key: {Key}", key);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing cache key: {Key}", key);
            return false;
        }
    }

    public async Task<bool> ExistsAsync(string key)
    {
        var fullKey = GetFullKey(key);
        var value = await _distributedCache.GetStringAsync(fullKey);
        return value != null;
    }

    public Task ClearAsync()
    {
        Clear();
        return Task.CompletedTask;
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
        var fullKey = GetFullKey(key);
        await _distributedCache.RefreshAsync(fullKey);
    }

    public async Task<bool> SetMultipleAsync<T>(
        Dictionary<string, T> items,
        TimeSpan? expiration = null
    )
    {
        var tasks = new List<Task<bool>>();
        foreach (var item in items)
        {
            tasks.Add(SetAsync(item.Key, item.Value, expiration));
        }

        var results = await Task.WhenAll(tasks);
        return Array.TrueForAll(results, r => r);
    }

    public async Task<Dictionary<string, T>> GetMultipleAsync<T>(IEnumerable<string> keys)
    {
        var result = new Dictionary<string, T>();
        var tasks = new List<Task<(string key, T value)>>();

        foreach (var key in keys)
        {
            tasks.Add(GetKeyValueAsync<T>(key));
        }

        var results = await Task.WhenAll(tasks);
        foreach (var (key, value) in results)
        {
            if (value != null)
                result[key] = value;
        }

        return result;
    }

    private async Task<(string key, T value)> GetKeyValueAsync<T>(string key)
    {
        var value = await GetAsync<T>(key);
        return (key, value);
    }

    private string GetFullKey(string key) => $"{_options.KeyPrefix}{key}";
}
