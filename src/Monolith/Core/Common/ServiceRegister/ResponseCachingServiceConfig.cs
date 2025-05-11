using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Common.ServiceRegister;

/// <summary>
/// Response caching service config.
/// </summary>
internal static class ResponseCachingServiceConfig
{
    /// <summary>
    /// Configure the response caching service.
    /// </summary>
    /// <param name="services"></param>
    internal static void AddResponseCachings(this IServiceCollection services)
    {
        services.AddResponseCaching();
    }
}
