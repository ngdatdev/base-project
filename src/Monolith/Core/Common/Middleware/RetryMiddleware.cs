using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Common.Middleware;

/// <summary>
/// Retry Middleware
/// </summary>
public sealed class RetryMiddleware
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly RequestDelegate _next;
    private readonly int _maxRetries;
    private readonly TimeSpan _retryDelay;
    private readonly ILogger<RetryMiddleware> _logger;

    public RetryMiddleware(RequestDelegate next, IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        var retryCount = 0;

        while (retryCount <= _maxRetries)
        {
            try
            {
                await _next(httpContext);
                return;
            }
            catch (Exception ex)
            {
                retryCount++;

                if (retryCount > _maxRetries)
                {
                    _logger?.LogError(ex, $"Server failed after {_maxRetries} retries");
                    throw;
                }

                await Task.Delay(_retryDelay);
            }
        }
    }
}
