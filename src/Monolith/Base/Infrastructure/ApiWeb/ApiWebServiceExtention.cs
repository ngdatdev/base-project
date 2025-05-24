using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.ApiWeb;

/// <summary>
/// The ApiWeb Service Extention
/// </summary>
internal static class ApiWebServiceExtention
{
    public static IServiceCollection AddApiWebConfig(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.Configure<BaseEndpointUrlOption>(
            configuration.GetRequiredSection("BaseEndpointUrl")
        );
        services.AddSingleton(
            configuration.GetRequiredSection("BaseEndpointUrl").Get<BaseEndpointUrlOption>()
        );

        return services;
    }
}
