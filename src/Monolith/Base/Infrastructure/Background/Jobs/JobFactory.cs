using System;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;

namespace Infrastructure.Background.Jobs;

/// <summary>
/// Custom job factory
/// </summary>
public class JobFactory : IJobFactory
{
    private readonly IServiceProvider _serviceProvider;

    public JobFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
    {
        using var scope = _serviceProvider.CreateScope();
        return scope.ServiceProvider.GetRequiredService(bundle.JobDetail.JobType) as IJob;
    }

    public void ReturnJob(IJob job)
    {
        if (job is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }
}
