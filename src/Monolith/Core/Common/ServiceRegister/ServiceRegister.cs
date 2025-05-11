using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text.RegularExpressions;
using Common.Features;
using Common.ServiceRegister;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.RegisterServices;

/// <summary>
/// Register common services.
/// </summary>
public static class ServiceRegister
{
    public static IServiceCollection RegisterCore(
        this IServiceCollection services,
        IConfigurationManager configuration
    )
    {
        // Register common services
        AddDispatcherService(services);
        AddHandlerService(services);

        // Register custom api services
        services.AddBaseController();
        services.AddAuthentication(configuration);
        services.AddAuthorizations();
        services.AddCORS();
        services.AddLoggings();
        services.AddRateLimiter(configuration);
        services.AddResponseCachings();
        services.AddSwagger(configuration);

        return services;
    }

    /// <summary>
    /// Add dispatcher service.
    /// </summary>
    /// <param name="services"></param>
    public static void AddDispatcherService(IServiceCollection services)
    {
        services.AddScoped<IDispatcher, Dispatcher>();
    }

    /// <summary>
    /// Add handler service for all handlers.
    /// </summary>
    /// <param name="services"></param>
    public static void AddHandlerService(IServiceCollection services)
    {
        string basePath = AppDomain.CurrentDomain.BaseDirectory;

        // Regex pattern: "F" + 3 number + ".dll"
        Regex regex = new Regex(@"F\d{3}\.dll$", RegexOptions.IgnoreCase);

        var dllFiles = Directory
            .GetFiles(basePath, "*.dll")
            .Where(file => regex.IsMatch(Path.GetFileName(file)))
            .ToList();

        List<Assembly> assemblies = new();
        foreach (var dll in dllFiles)
        {
            try
            {
                Assembly assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(dll);
                assemblies.Add(assembly);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to load assembly: {dll}", ex);
            }
        }

        foreach (var assembly in assemblies)
        {
            var handlerTypes = assembly
                .GetTypes()
                .Where(t =>
                    !t.IsAbstract
                    && t.GetInterfaces()
                        .Any(i =>
                            i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IHandler<,>)
                        )
                );

            foreach (var handlerType in handlerTypes)
            {
                var handlerInterfaces = handlerType
                    .GetInterfaces()
                    .Where(i =>
                        i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IHandler<,>)
                    );

                foreach (var handlerInterface in handlerInterfaces)
                {
                    services.AddTransient(handlerInterface, handlerType);
                }
            }
        }
    }
}
