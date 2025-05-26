using System.Threading.Tasks;
using BaseApiReference.Abstractions.Background;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Background.Services;

/// <summary>
/// Dummy report service
/// </summary>
public class ReportService : IReportService
{
    private readonly ILogger<ReportService> _logger;

    public ReportService(ILogger<ReportService> logger)
    {
        _logger = logger;
    }

    public async Task<byte[]> GenerateReportAsync(string reportType, string department)
    {
        _logger.LogInformation($"Generating {reportType} report for {department}");
        await Task.Delay(5000);
        return new byte[] { 1, 2, 3, 4, 5 };
    }

    public async Task SaveReportAsync(byte[] reportData, string fileName)
    {
        _logger.LogInformation($"Saving report: {fileName}");
        await Task.Delay(1000);
    }
}
