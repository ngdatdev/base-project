using Infrastructure.Caching;
using Infrastructure.IdGenerator;
using Infrastructure.Persistence.PostgreSQL.Extentions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.ServiceRegister;

/// <summary>
/// Service Register.
/// </summary>
public static class ServiceRegister
{
    /// <summary>
    /// Register Infrastructure
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public static void RegisterInfrastructure(
        this IServiceCollection services,
        IConfigurationManager configuration
    )
    {
        services.AddIdGenerator();
        services.AddCaching(configuration);
        services.AddDatabaseContextPool(configuration);
        services.AddAspNetCoreIdentity(configuration);
    }
}
