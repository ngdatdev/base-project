using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BaseApiReference.Extentions;

/// <summary>
/// Interface for service registration.
/// </summary>
public interface IServiceRegister
{
    /// <summary>
    /// Register services for dependency injection.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    IServiceCollection Register(IServiceCollection services, IConfiguration configuration);
}
