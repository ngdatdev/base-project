using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Infrastructure.Background.Jobs;

public class CustomSchedulerListener : ISchedulerListener
{
    private readonly ILogger<CustomSchedulerListener> _logger;

    public CustomSchedulerListener(ILogger<CustomSchedulerListener> logger)
    {
        _logger = logger;
    }

    public async Task JobScheduled(ITrigger trigger, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            $"Job scheduled: {trigger.JobKey.Name} with trigger: {trigger.Key.Name}"
        );
        await Task.CompletedTask;
    }

    public async Task JobUnscheduled(
        TriggerKey triggerKey,
        CancellationToken cancellationToken = default
    )
    {
        _logger.LogInformation($"Job unscheduled: {triggerKey.Name}");
        await Task.CompletedTask;
    }

    public async Task TriggerFinalized(
        ITrigger trigger,
        CancellationToken cancellationToken = default
    )
    {
        _logger.LogInformation($"Trigger finalized: {trigger.Key.Name}");
        await Task.CompletedTask;
    }

    public async Task TriggerPaused(
        TriggerKey triggerKey,
        CancellationToken cancellationToken = default
    )
    {
        _logger.LogInformation($"Trigger paused: {triggerKey.Name}");
        await Task.CompletedTask;
    }

    public async Task TriggersPaused(
        string? triggerGroup,
        CancellationToken cancellationToken = default
    )
    {
        _logger.LogInformation($"Triggers paused in group: {triggerGroup ?? "ALL"}");
        await Task.CompletedTask;
    }

    public async Task TriggerResumed(
        TriggerKey triggerKey,
        CancellationToken cancellationToken = default
    )
    {
        _logger.LogInformation($"Trigger resumed: {triggerKey.Name}");
        await Task.CompletedTask;
    }

    public async Task TriggersResumed(
        string? triggerGroup,
        CancellationToken cancellationToken = default
    )
    {
        _logger.LogInformation($"Triggers resumed in group: {triggerGroup ?? "ALL"}");
        await Task.CompletedTask;
    }

    public async Task JobAdded(IJobDetail jobDetail, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation($"Job added: {jobDetail.Key.Name}");
        await Task.CompletedTask;
    }

    public async Task JobDeleted(JobKey jobKey, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation($"Job deleted: {jobKey.Name}");
        await Task.CompletedTask;
    }

    public async Task JobPaused(JobKey jobKey, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation($"Job paused: {jobKey.Name}");
        await Task.CompletedTask;
    }

    public async Task JobsPaused(string jobGroup, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation($"Jobs paused in group: {jobGroup}");
        await Task.CompletedTask;
    }

    public async Task JobResumed(JobKey jobKey, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation($"Job resumed: {jobKey.Name}");
        await Task.CompletedTask;
    }

    public async Task JobsResumed(string jobGroup, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation($"Jobs resumed in group: {jobGroup}");
        await Task.CompletedTask;
    }

    public async Task SchedulerError(
        string msg,
        SchedulerException cause,
        CancellationToken cancellationToken = default
    )
    {
        _logger.LogError(cause, $"Scheduler error: {msg}");
        await Task.CompletedTask;
    }

    public async Task SchedulerInStandbyMode(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Scheduler in standby mode");
        await Task.CompletedTask;
    }

    public async Task SchedulerStarted(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Scheduler started");
        await Task.CompletedTask;
    }

    public async Task SchedulerStarting(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Scheduler starting");
        await Task.CompletedTask;
    }

    public async Task SchedulerShutdown(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Scheduler shutdown");
        await Task.CompletedTask;
    }

    public async Task SchedulerShuttingdown(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Scheduler shutting down");
        await Task.CompletedTask;
    }

    public async Task SchedulingDataCleared(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Scheduling data cleared");
        await Task.CompletedTask;
    }

    public async Task JobInterrupted(JobKey jobKey, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Scheduling data cleared");
        await Task.CompletedTask;
    }
}
