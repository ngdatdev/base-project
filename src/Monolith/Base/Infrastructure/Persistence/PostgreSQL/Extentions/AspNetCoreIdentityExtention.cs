using System;
using BaseApiReference.Entities;
using Infrastructure.Persistence.PostgreSQL.DbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence.PostgreSQL.Extentions;

/// <summary>
///     AspNetCoreIdentity service config.
/// </summary>
internal static class AspNetCoreIdentityExtention
{
    /// <summary>
    ///     Configure asp net core identity service.
    /// </summary>
    /// <param name="services">
    ///     Service container.
    /// </param>
    /// <param name="configuration">
    ///     Load configuration for configuration
    ///     file (appsetting).
    /// </param>
    internal static void AddAspNetCoreIdentity(
        this IServiceCollection services,
        IConfigurationManager configuration
    )
    {
        services
            .AddIdentity<User, Role>(setupAction: config =>
            {
                var option = configuration
                    .GetRequiredSection(key: "AspNetCoreIdentity")
                    .Get<AspNetCoreIdentityOption>();

                config.Password.RequireDigit = option.Password.RequireDigit;
                config.Password.RequireLowercase = option.Password.RequireLowercase;
                config.Password.RequireNonAlphanumeric = option.Password.RequireNonAlphanumeric;
                config.Password.RequireUppercase = option.Password.RequireUppercase;
                config.Password.RequiredLength = option.Password.RequiredLength;
                config.Password.RequiredUniqueChars = option.Password.RequiredUniqueChars;

                config.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(
                    value: option.Lockout.DefaultLockoutTimeSpanInSecond
                );
                config.Lockout.MaxFailedAccessAttempts = option.Lockout.MaxFailedAccessAttempts;
                config.Lockout.AllowedForNewUsers = option.Lockout.AllowedForNewUsers;

                config.User.AllowedUserNameCharacters = option.User.AllowedUserNameCharacters;
                config.User.RequireUniqueEmail = option.User.RequireUniqueEmail;

                config.SignIn.RequireConfirmedEmail = option.SignIn.RequireConfirmedEmail;
                config.SignIn.RequireConfirmedPhoneNumber = option
                    .SignIn
                    .RequireConfirmedPhoneNumber;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
    }
}
