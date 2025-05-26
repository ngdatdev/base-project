using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseApiReference.Abstractions.Background;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Infrastructure.Background.Jobs;

/// <summary>
/// Report generation job
/// </summary>
[DisallowConcurrentExecution]
public class ReportGenerationJob : BaseJob
{
    private readonly IReportService _reportService;

    public ReportGenerationJob(
        ILogger<ReportGenerationJob> logger,
        IServiceProvider serviceProvider,
        IReportService reportService
    )
        : base(logger, serviceProvider)
    {
        _reportService = reportService;
    }

    public override async Task Execute(IJobExecutionContext context)
    {
        await LogJobExecution(
            context,
            async () =>
            {
                var jobData = context.JobDetail.JobDataMap;
                var reportType = jobData.GetString("ReportType") ?? "Default";
                var department = jobData.GetString("Department") ?? "All";

                await ExecuteWithRetry(async () =>
                {
                    var reportData = await _reportService.GenerateReportAsync(
                        reportType,
                        department
                    );
                    var fileName = $"{reportType}_{department}_{DateTime.UtcNow:yyyyMMdd}.pdf";

                    await _reportService.SaveReportAsync(reportData, fileName);
                    return true;
                });
            }
        );
    }
}
