using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Infrastructure.Background.Jobs;

public class CustomJobListener : IJobListener
{
    private readonly ILogger<CustomJobListener> _logger;
    private readonly IServiceProvider _serviceProvider;

    public string Name => "CustomJobListener";

    public CustomJobListener(ILogger<CustomJobListener> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public async Task JobToBeExecuted(
        IJobExecutionContext context,
        CancellationToken cancellationToken = default
    )
    {
        var jobName = context.JobDetail.Key.Name;
        _logger.LogInformation($"Job {jobName} is about to be executed");

        // Record job start in database
        await RecordJobExecution(context, "Started", null);
    }

    public async Task JobExecutionVetoed(
        IJobExecutionContext context,
        CancellationToken cancellationToken = default
    )
    {
        var jobName = context.JobDetail.Key.Name;
        _logger.LogWarning($"Job {jobName} execution was vetoed");

        await RecordJobExecution(context, "Vetoed", null);
    }

    public async Task JobWasExecuted(
        IJobExecutionContext context,
        JobExecutionException? jobException,
        CancellationToken cancellationToken = default
    )
    {
        var jobName = context.JobDetail.Key.Name;
        var duration = context.JobRunTime;

        if (jobException != null)
        {
            _logger.LogError(
                jobException,
                $"Job {jobName} failed after {duration.TotalSeconds:F2} seconds"
            );
            await RecordJobExecution(context, "Failed", jobException.Message);
            await HandleJobFailure(context, jobException);
        }
        else
        {
            _logger.LogInformation(
                $"Job {jobName} completed successfully in {duration.TotalSeconds:F2} seconds"
            );
            await RecordJobExecution(context, "Success", null);
        }
    }

    private async Task RecordJobExecution(
        IJobExecutionContext context,
        string status,
        string? errorMessage
    )
    {
        try
        {
            // In a real implementation, save to database
            //var executionRecord = new JobExecutionHistory
            //{
            //    ExecutionId = Guid.NewGuid().ToString(),
            //    JobName = context.JobDetail.Key.Name,
            //    JobGroup = context.JobDetail.Key.Group,
            //    StartTime = context.FireTime.DateTime,
            //    EndTime = status != "Started" ? DateTime.UtcNow : null,
            //    Status = status,
            //    ErrorMessage = errorMessage
            //};

            // Simulate database save
            //_logger.LogDebug(
            //    $"Recording job execution: {JsonSerializer.Serialize(executionRecord)}"
            //);
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to record job execution");
        }
    }

    private async Task HandleJobFailure(
        IJobExecutionContext context,
        JobExecutionException jobException
    )
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            //var notificationService = scope.ServiceProvider.GetService<INotificationService>();

            //if (notificationService != null)
            //{
            //    var jobName = context.JobDetail.Key.Name;
            //    await notificationService.SendPushNotificationAsync(
            //        "admin",
            //        $"Job Failed: {jobName} - {jobException.Message}"
            //    );
            //}
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send job failure notification");
        }
    }
}
