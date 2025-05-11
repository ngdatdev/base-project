using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Common.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Common.ServiceRegister;

/// <summary>
/// Authentication service config.
/// </summary>
internal static class AuthenticationServiceConfig
{
    /// <summary>
    /// Config authentication.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    internal static void AddAuthentication(
        this IServiceCollection services,
        IConfigurationManager configuration
    )
    {
        JsonWebTokenHandler.DefaultInboundClaimTypeMap.Clear();
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

        var option = configuration
            .GetRequiredSection(key: "Authentication")
            .Get<JwtAuthenticationOption>();

        TokenValidationParameters tokenValidationParameters =
            new()
            {
                ValidateIssuer = option.Jwt.ValidateIssuer,
                ValidateAudience = option.Jwt.ValidateAudience,
                ValidateLifetime = option.Jwt.ValidateLifetime,
                ValidateIssuerSigningKey = option.Jwt.ValidateIssuerSigningKey,
                RequireExpirationTime = option.Jwt.RequireExpirationTime,
                ValidTypes = option.Jwt.ValidTypes,
                ValidIssuer = option.Jwt.ValidIssuer,
                ValidAudience = option.Jwt.ValidAudience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    key: new HMACSHA256(
                        key: Encoding.UTF8.GetBytes(s: option.Jwt.IssuerSigningKey)
                    ).Key
                )
            };

        services
            .AddSingleton(implementationInstance: option)
            .AddSingleton(implementationInstance: tokenValidationParameters)
            .AddAuthentication(configureOptions: config =>
            {
                config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(configureOptions: config =>
            {
                config.TokenValidationParameters = tokenValidationParameters;

                config.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var path = context.HttpContext.Request.Path;
                        if (path.StartsWithSegments("/chat-hub"))
                        {
                            var token = context.Request.Query["token"];
                            if (!string.IsNullOrEmpty(token))
                            {
                                context.Token = token;
                            }
                        }
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        return Task.CompletedTask;
                    }
                };
            });
    }
}
