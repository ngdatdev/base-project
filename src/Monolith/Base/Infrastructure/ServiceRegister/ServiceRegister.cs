using Infrastructure.Caching;
using Infrastructure.IdGenerator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.ServiceRegister;

/// <summary>
/// Service Register.
/// </summary>
public static class ServiceRegister
{
    public static void RegisterInfrastructure(
        this IServiceCollection services,
        IConfigurationManager configuration
    )
    {
        services.AddIdGenerator();
        services.AddCaching(configuration);
    }
}
