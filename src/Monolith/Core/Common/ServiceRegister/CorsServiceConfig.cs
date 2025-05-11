using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Common.ServiceRegister;

/// <summary>
/// Cors service config.
/// </summary>
public static class CorsServiceConfig
{
    /// <summary>
    /// Configure cors.
    /// </summary>
    /// <param name="services"></param>
    public static void AddCORS(this IServiceCollection services)
    {
        services.AddCors(setupAction: config =>
        {
            config.AddDefaultPolicy(configurePolicy: policy =>
            {
                policy
                    //.WithOrigins(
                    //    "http://localhost:3000",
                    //)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
    }
}
