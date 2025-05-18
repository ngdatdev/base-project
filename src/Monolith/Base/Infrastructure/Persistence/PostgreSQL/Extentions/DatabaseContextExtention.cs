using System.Reflection;
using Infrastructure.Persistence.PostgreSQL.Data;
using Infrastructure.Persistence.PostgreSQL.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence.PostgreSQL.Extentions;

/// <summary>
/// PostgreSQL DbContext Pool service config.
/// </summary>
public static class DatabaseContextExtention
{
    /// <summary>
    /// Configure the db context pool service.
    /// </summary>
    /// <param name="services">
    /// Service container.
    /// </param>
    /// <param name="configuration">
    /// Load configuration for configuration
    /// file (appsetting).
    /// </param>
    internal static void AddDatabaseContextPool(
        this IServiceCollection services,
        IConfigurationManager configuration
    )
    {
        services.AddDbContext<AppDbContext>(
            optionsAction: (provider, config) =>
            {
                var baseDatabaseOption = configuration
                    .GetRequiredSection("Database")
                    .GetRequiredSection("SecurityModule")
                    .Get<DatabaseOption>();

                config
                    .UseNpgsql(
                        connectionString: baseDatabaseOption?.ConnectionString,
                        npgsqlOptionsAction: databaseOptionsAction =>
                        {
                            databaseOptionsAction
                                .CommandTimeout(commandTimeout: baseDatabaseOption?.CommandTimeOut)
                                .EnableRetryOnFailure(
                                    maxRetryCount: baseDatabaseOption.EnableRetryOnFailure
                                )
                                .MigrationsAssembly(
                                    assemblyName: Assembly.GetExecutingAssembly().GetName().Name
                                );
                        }
                    )
                    .EnableSensitiveDataLogging(
                        sensitiveDataLoggingEnabled: baseDatabaseOption.EnableSensitiveDataLogging
                    )
                    .EnableDetailedErrors(
                        detailedErrorsEnabled: baseDatabaseOption.EnableDetailedErrors
                    )
                    .EnableThreadSafetyChecks(
                        enableChecks: baseDatabaseOption.EnableThreadSafetyChecks
                    )
                    .EnableServiceProviderCaching(
                        cacheServiceProvider: baseDatabaseOption.EnableServiceProviderCaching
                    );
            }
        );
        services.AddDbContextFactory<AppDbContext>(
            optionsAction: (provider, config) =>
            {
                var option = configuration
                    .GetRequiredSection(key: "Database")
                    .GetRequiredSection(key: "SecurityModule")
                    .Get<DatabaseOption>();

                config
                    .UseNpgsql(
                        connectionString: option?.ConnectionString,
                        npgsqlOptionsAction: databaseOptionsAction =>
                        {
                            databaseOptionsAction
                                .CommandTimeout(commandTimeout: option?.CommandTimeOut)
                                .EnableRetryOnFailure(maxRetryCount: option.EnableRetryOnFailure)
                                .MigrationsAssembly(
                                    assemblyName: Assembly.GetExecutingAssembly().GetName().Name
                                );
                        }
                    )
                    .EnableSensitiveDataLogging(
                        sensitiveDataLoggingEnabled: option.EnableSensitiveDataLogging
                    )
                    .EnableDetailedErrors(detailedErrorsEnabled: option.EnableDetailedErrors)
                    .EnableThreadSafetyChecks(enableChecks: option.EnableThreadSafetyChecks)
                    .EnableServiceProviderCaching(
                        cacheServiceProvider: option.EnableServiceProviderCaching
                    );
            },
            ServiceLifetime.Scoped
        );
        services.AddSingleton<DataSeeding>();
    }
}
