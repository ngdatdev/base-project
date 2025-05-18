using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace BaseApiReference.Extentions;

/// <summary>
/// Extension methods for service collection.
/// </summary>
public static class ServiceCollectionExtension
{
    public static IServiceCollection MakeSingletonLazy<T>(this IServiceCollection services)
        where T : class
    {
        return services.AddSingleton<Lazy<T>>(provider => new(provider.GetRequiredService<T>()));
    }

    public static IServiceCollection MakeScopedLazy<T>(this IServiceCollection services)
        where T : class
    {
        return services.AddScoped<Lazy<T>>(provider => new(provider.GetRequiredService<T>()));
    }

    public static IServiceCollection MakeTransientLazy<T>(this IServiceCollection services)
        where T : class
    {
        return services.AddTransient<Lazy<T>>(provider => new(provider.GetRequiredService<T>()));
    }
}
