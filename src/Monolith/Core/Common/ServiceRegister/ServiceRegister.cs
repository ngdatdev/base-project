using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text.RegularExpressions;
using Common.Features;
using Common.Filters;
using Common.ServiceRegister;
using FluentValidation;
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
        IConfigurationManager configuration,
        Assembly assembly
    )
    {
        // Register common services
        AddDispatcherService(services);
        AddHandlerService(services, configuration);
        AddValidation(services, configuration);

        // Register custom api services
        services.AddBaseController();
        services.AddAuthentication(configuration);
        services.AddAuthorizations();
        services.AddCORS();
        services.AddLoggings();
        services.AddRateLimiter(configuration);
        services.AddResponseCachings();
        services.AddSwagger(configuration, assembly);
        services.AddConfigFilter(configuration);
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
    public static void AddHandlerService(
        IServiceCollection services,
        IConfigurationManager configuration
    )
    {
        string basePath = AppDomain.CurrentDomain.BaseDirectory;

        // Regex pattern: "F" + 3 number + ".dll"
        var pattern = configuration.GetSection("RuleName").GetSection("PrefixFeature").Value;

        Regex regex = new Regex($@"{pattern}\.dll$", RegexOptions.IgnoreCase);

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

    /// <summary>
    /// Add validation.
    /// </summary>
    /// <param name="services"></param>
    public static void AddValidation(
        this IServiceCollection services,
        IConfigurationManager configuration
    )
    {
        var pattern = configuration.GetSection("RuleName").GetSection("PrefixFeature").Value;

        var assemblies = AppDomain
            .CurrentDomain.GetAssemblies()
            .Where(a => Regex.IsMatch(a.GetName().Name, pattern))
            .ToList();

        services.AddValidatorsFromAssembly(typeof(ServiceRegister).Assembly);

        foreach (var assembly in assemblies.Distinct())
        {
            services.AddValidatorsFromAssembly(assembly);
        }
    }
}
