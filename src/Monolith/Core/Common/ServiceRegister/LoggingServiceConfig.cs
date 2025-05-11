using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Common.ServiceRegister;

/// <summary>
/// Logging service config.
/// </summary>
internal static class LoggingServiceConfig
{
    /// <summary>
    /// Config logging.
    /// </summary>
    /// <param name="services"></param>
    internal static void AddLoggings(this IServiceCollection services)
    {
        services.AddLogging(configure: config =>
        {
            config.ClearProviders();
            config.AddConsole();
        });
    }
}
