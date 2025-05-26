using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Infrastructure.Background.Jobs;

public class CustomTriggerListener : ITriggerListener
{
    private readonly ILogger<CustomTriggerListener> _logger;

    public string Name => "CustomTriggerListener";

    public CustomTriggerListener(ILogger<CustomTriggerListener> logger)
    {
        _logger = logger;
    }

    public async Task TriggerFired(
        ITrigger trigger,
        IJobExecutionContext context,
        CancellationToken cancellationToken = default
    )
    {
        var triggerName = trigger.Key.Name;
        var jobName = context.JobDetail.Key.Name;
        _logger.LogDebug($"Trigger {triggerName} fired for job {jobName}");
        await Task.CompletedTask;
    }

    public async Task<bool> VetoJobExecution(
        ITrigger trigger,
        IJobExecutionContext context,
        CancellationToken cancellationToken = default
    )
    {
        // Implement business logic to veto job execution if needed
        // Return true to veto (prevent) the job execution

        var jobName = context.JobDetail.Key.Name;

        // Example: Don't run data sync during business hours on weekdays
        if (jobName.Contains("DataSync") && IsBusinessHours())
        {
            _logger.LogInformation($"Vetoing {jobName} execution during business hours");
            return true;
        }

        return false;
    }

    public async Task TriggerMisfired(
        ITrigger trigger,
        CancellationToken cancellationToken = default
    )
    {
        var triggerName = trigger.Key.Name;
        _logger.LogWarning($"Trigger {triggerName} misfired");
        await Task.CompletedTask;
    }

    public async Task TriggerComplete(
        ITrigger trigger,
        IJobExecutionContext context,
        SchedulerInstruction triggerInstructionCode,
        CancellationToken cancellationToken = default
    )
    {
        var triggerName = trigger.Key.Name;
        var jobName = context.JobDetail.Key.Name;
        _logger.LogDebug(
            $"Trigger {triggerName} completed for job {jobName} with instruction: {triggerInstructionCode}"
        );
        await Task.CompletedTask;
    }

    private bool IsBusinessHours()
    {
        var now = DateTime.Now;
        return now.DayOfWeek != DayOfWeek.Saturday
            && now.DayOfWeek != DayOfWeek.Sunday
            && now.Hour >= 9
            && now.Hour < 17;
    }
}
