using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Common.Filters;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.ServiceRegister;

/// <summary>
/// Filter service config.
/// </summary>
internal static class FilterServiceConfig
{
    /// <summary>
    ///     Entry to configuring multiple services.
    /// </summary>
    /// <param name="services">
    ///     Service container.
    /// </param>
    internal static void AddConfigFilter(
        this IServiceCollection services,
        IConfigurationManager configuration
    )
    {
        var assemblies = AppDomain
            .CurrentDomain.GetAssemblies()
            .Where(a =>
                a.GetName()
                    .Name.StartsWith(
                        configuration.GetSection("RuleName").GetSection("PrefixFeature").Value
                    )
            )
            .ToList();

        foreach (var assembly in assemblies.Distinct())
        {
            var filterActionTypes = assembly
                .GetTypes()
                .Where(t => typeof(IAsyncActionFilter).IsAssignableFrom(t) && !t.IsAbstract);

            foreach (var filterType in filterActionTypes)
            {
                services.AddScoped(filterType);
            }

            var filterAuthorizationTypes = assembly
                .GetTypes()
                .Where(t => typeof(IAsyncAuthorizationFilter).IsAssignableFrom(t) && !t.IsAbstract);

            foreach (var filterType in filterAuthorizationTypes)
            {
                services.AddScoped(filterType);
            }

            services.AddScoped(typeof(ValidationRequestFilter<>));

            services.AddScoped(typeof(AuthorizationFilter));
        }
    }
}
