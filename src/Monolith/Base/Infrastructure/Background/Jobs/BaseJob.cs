using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Infrastructure.Background.Jobs;

/// <summary>
/// Base class for all jobs
/// </summary>
public abstract class BaseJob : IJob
{
    protected readonly ILogger _logger;
    protected readonly IServiceProvider _serviceProvider;

    protected BaseJob(ILogger logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public abstract Task Execute(IJobExecutionContext context);

    protected async Task<T> ExecuteWithRetry<T>(Func<Task<T>> operation, int maxRetries = 3)
    {
        for (int i = 0; i < maxRetries; i++)
        {
            try
            {
                return await operation();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Operation failed on attempt {i + 1}");
                if (i == maxRetries - 1)
                    throw;
                await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, i))); // Exponential backoff
            }
        }
        throw new InvalidOperationException("Should not reach here");
    }

    protected async Task LogJobExecution(IJobExecutionContext context, Func<Task> jobLogic)
    {
        var jobName = context.JobDetail.Key.Name;
        var startTime = DateTime.UtcNow;

        _logger.LogInformation($"Starting job: {jobName} at {startTime}");

        try
        {
            await jobLogic();
            var duration = DateTime.UtcNow - startTime;
            _logger.LogInformation(
                $"Job {jobName} completed successfully in {duration.TotalSeconds:F2} seconds"
            );
        }
        catch (Exception ex)
        {
            var duration = DateTime.UtcNow - startTime;
            _logger.LogError(ex, $"Job {jobName} failed after {duration.TotalSeconds:F2} seconds");
            throw;
        }
    }
}
