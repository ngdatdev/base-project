using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Common.ServiceRegister;

/// <summary>
/// Authorization service config.
/// </summary>
public static class AuthorizationServiceConfig
{
    /// <summary>
    /// Configure authorization.
    /// </summary>
    /// <param name="services"></param>
    internal static void AddAuthorizations(this IServiceCollection services)
    {
        services.AddAuthorization();
    }
}
