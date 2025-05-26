using System;
using Infrastructure.Background.Jobs;
using Infrastructure.Persistence.PostgreSQL.Extentions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace Infrastructure.Background;

/// <summary>
/// Quartz services extension
/// </summary>
internal static class QuartzServicesExtension
{
    public static IServiceCollection AddBackgroundServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var baseDatabaseOption = configuration
            .GetRequiredSection("Database")
            .GetRequiredSection("SecurityModule")
            .Get<DatabaseOption>();

        services.AddQuartz(q =>
        {
            // Use a Scoped container
            q.UseMicrosoftDependencyInjectionJobFactory();

            // JSON Serialization
            q.UseSimpleTypeLoader();
            q.UseInMemoryStore();
            q.UseDefaultThreadPool(tp =>
            {
                tp.MaxConcurrency = 10;
            });

            q.UsePersistentStore(s =>
            {
                s.UseProperties = true;
                s.RetryInterval = TimeSpan.FromSeconds(15);
                s.UsePostgres(connectionString =>
                {
                    connectionString.ConnectionString = baseDatabaseOption?.ConnectionString;
                    connectionString.TablePrefix = "QRTZ_";
                });
                s.UseJsonSerializer();
                s.UseClustering(c =>
                {
                    c.CheckinMisfireThreshold = TimeSpan.FromSeconds(20);
                    c.CheckinInterval = TimeSpan.FromSeconds(10);
                });
            });
            q.AddJobListener<CustomJobListener>();
            q.AddTriggerListener<CustomTriggerListener>();
            q.AddSchedulerListener<CustomSchedulerListener>();

            ConfigureJobs(q);
        });

        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
        return services;
    }

    private static void ConfigureJobs(IServiceCollectionQuartzConfigurator q)
    {
        var emailJobKey = new JobKey("EmailJob", "EmailGroup");
        q.AddJob<ReportGenerationJob>(opts => opts.WithIdentity(emailJobKey));
        q.AddTrigger(opts =>
            opts.ForJob(emailJobKey)
                .WithIdentity("EmailJob-trigger", "EmailGroup")
                .WithCronSchedule("0 1 * * * ?")
        );
    }
}
