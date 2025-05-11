using Microsoft.Extensions.DependencyInjection;

namespace Common.ServiceRegister;

/// <summary>
/// Entry point service config.
/// </summary>
internal static class EntryPointServiceConfig
{
    /// <summary>
    /// Add base controller.
    /// </summary>
    /// <param name="services"></param>
    internal static void AddBaseController(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
    }
}
