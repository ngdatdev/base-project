using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text.RegularExpressions;
using BaseApiReference.Abstractions.Tokens;
using Common.Applications.Repositories;
using Common.Applications.Tokens;
using Common.Features;
using Common.ServiceRegister;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;

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
        LoadAssemblies(configuration);
        AddDispatcherService(services);
        AddValidation(services, configuration);
        AddTokenHandlers(services);
        AddRepositories(services, configuration);
        AddApplications(services);
        AddHandlerService(services, configuration);

        // Register custom api services
        services.AddHttpClient();
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

    private static void LoadAssemblies(IConfigurationManager configuration)
    {
        string basePath = AppDomain.CurrentDomain.BaseDirectory;
        var pattern = configuration.GetSection("RuleName").GetSection("PrefixFeature").Value;
        foreach (string dll in Directory.GetFiles(basePath, "*.dll"))
        {
            try
            {
                var assemblyName = AssemblyName.GetAssemblyName(dll);
                if (!Regex.IsMatch(assemblyName.Name, pattern))
                    continue;

                if (
                    !AppDomain
                        .CurrentDomain.GetAssemblies()
                        .Any(a => a.FullName == assemblyName.FullName)
                )
                {
                    Assembly.Load(assemblyName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Can't load {dll}: {ex.Message}");
            }
        }
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

        var assemblies = AppDomain
            .CurrentDomain.GetAssemblies()
            .Where(a => Regex.IsMatch(a.GetName().Name, pattern))
            .ToList();

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

    /// <summary>
    /// Add token handlers.
    /// </summary>
    /// <param name="services"></param>
    public static void AddTokenHandlers(this IServiceCollection services)
    {
        services.AddSingleton<JsonWebTokenHandler>();
        services.AddSingleton<IAccessTokenHandler, AccessTokenHandler>();
        services.AddSingleton<IRefreshTokenHandler, RefreshTokenHandler>();
        services.AddSingleton<IOTPHandler, OTPHandler>();
    }

    /// <summary>
    /// Add applications.
    /// </summary>
    /// <param name="services"></param>
    public static void AddApplications(this IServiceCollection services)
    {
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    public static void AddRepositories(
        this IServiceCollection services,
        IConfigurationManager configuration
    )
    {
        var pattern = configuration.GetSection("RuleName")["PrefixFeature"];
        if (string.IsNullOrWhiteSpace(pattern))
            throw new ArgumentException("Missing PrefixFeature regex pattern in configuration.");

        var interfaceRegex = new Regex(@"^I\w+Repository$");
        var implRegex = new Regex(@"^(\w+)Repository$");

        var assemblies = AppDomain
            .CurrentDomain.GetAssemblies()
            .Where(a => Regex.IsMatch(a.GetName().Name, pattern))
            .ToList();

        foreach (var assembly in assemblies)
        {
            Type[] types = assembly.GetTypes();

            var interfaces = types.Where(t => t.IsInterface && interfaceRegex.IsMatch(t.Name));
            foreach (var @interface in interfaces)
            {
                var match = interfaceRegex.Match(@interface.Name);
                if (!match.Success)
                    continue;

                var key = match.Groups[1].Value;
                var implName = key + "Repository";

                var implementation = types.FirstOrDefault(t =>
                    t.IsClass
                    && !t.IsAbstract
                    && implRegex.IsMatch(t.Name)
                    && @interface.IsAssignableFrom(t)
                );

                if (implementation != null)
                {
                    services.AddScoped(@interface, implementation);
                }
            }
        }
    }
}
