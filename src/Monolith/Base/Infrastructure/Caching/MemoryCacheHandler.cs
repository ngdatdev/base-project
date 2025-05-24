using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BaseApiReference.Abstractions.Caching;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Caching;

/// <summary>
/// Implementation of cache handler using memory as storage.
/// </summary>
public class MemoryCacheHandler : ICacheHandler
{
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<MemoryCacheHandler> _logger;
    private readonly MemoryCacheOption _options;
    private readonly HashSet<string> _cacheKeys;

    public MemoryCacheHandler(
        IMemoryCache memoryCache,
        ILogger<MemoryCacheHandler> logger,
        IOptions<MemoryCacheOption> options
    )
    {
        _memoryCache = memoryCache;
        _logger = logger;
        _options = options.Value;
        _cacheKeys = new HashSet<string>();
    }

    public T Get<T>(string key)
    {
        try
        {
            var fullKey = GetFullKey(key);
            var result = _memoryCache.Get<T>(fullKey);

            if (_options.EnableLogging)
            {
                if (result != null)
                    _logger.LogDebug("Cache hit for key: {Key}", key);
                else
                    _logger.LogDebug("Cache miss for key: {Key}", key);
            }

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
            var exp = expiration ?? _options.DefaultExpiration;

            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = exp,
                Priority = CacheItemPriority.Normal
            };

            if (_options.SizeLimit.HasValue)
            {
                cacheEntryOptions.Size = GetObjectSize(value);
            }

            _memoryCache.Set(fullKey, value, cacheEntryOptions);
            _cacheKeys.Add(fullKey);

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
            _memoryCache.Remove(fullKey);
            _cacheKeys.Remove(fullKey);

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
        return _memoryCache.TryGetValue(fullKey, out _);
    }

    public void Clear()
    {
        try
        {
            foreach (var key in _cacheKeys.ToList())
            {
                _memoryCache.Remove(key);
            }
            _cacheKeys.Clear();

            if (_options.EnableLogging)
                _logger.LogDebug("Cleared all cache entries");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error clearing cache");
        }
    }

    public Task<T> GetAsync<T>(string key) => Task.FromResult(Get<T>(key));

    public Task<bool> SetAsync<T>(string key, T value, TimeSpan? expiration = null) =>
        Task.FromResult(Set(key, value, expiration));

    public Task<bool> RemoveAsync(string key) => Task.FromResult(Remove(key));

    public Task<bool> ExistsAsync(string key) => Task.FromResult(Exists(key));

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
        var cached = Get<T>(key);
        if (cached != null)
            return cached;

        var value = await factory();
        if (value != null)
            Set(key, value, expiration);
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

    public Task RefreshAsync(string key)
    {
        // Memory cache doesn't support refresh, just return completed task
        return Task.CompletedTask;
    }

    public async Task<bool> SetMultipleAsync<T>(
        Dictionary<string, T> items,
        TimeSpan? expiration = null
    )
    {
        var success = true;
        foreach (var item in items)
        {
            if (!Set(item.Key, item.Value, expiration))
                success = false;
        }
        return success;
    }

    public Task<Dictionary<string, T>> GetMultipleAsync<T>(IEnumerable<string> keys)
    {
        var result = new Dictionary<string, T>();
        foreach (var key in keys)
        {
            var value = Get<T>(key);
            if (value != null)
                result[key] = value;
        }
        return Task.FromResult(result);
    }

    private string GetFullKey(string key) => $"{_options.KeyPrefix}{key}";

    private long GetObjectSize<T>(T obj)
    {
        try
        {
            return JsonSerializer.Serialize(obj).Length;
        }
        catch
        {
            return 1;
        }
    }
}
