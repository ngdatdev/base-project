using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BaseApiReference.Abstractions.Caching;

/// <summary>
/// Represent interface of caching handler.
/// </summary>
public interface ICacheHandler
{
    // Synchronous methods
    T Get<T>(string key);
    bool Set<T>(string key, T value, TimeSpan? expiration = null);
    bool Remove(string key);
    bool Exists(string key);
    void Clear();

    // Asynchronous methods
    Task<T> GetAsync<T>(string key);
    Task<bool> SetAsync<T>(string key, T value, TimeSpan? expiration = null);
    Task<bool> RemoveAsync(string key);
    Task<bool> ExistsAsync(string key);
    Task ClearAsync();

    // Advanced methods
    Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null);
    T GetOrSet<T>(string key, Func<T> factory, TimeSpan? expiration = null);
    Task RefreshAsync(string key);
    Task<bool> SetMultipleAsync<T>(Dictionary<string, T> items, TimeSpan? expiration = null);
    Task<Dictionary<string, T>> GetMultipleAsync<T>(IEnumerable<string> keys);
}
