using System.Threading.Tasks;

namespace BaseApiReference.Abstractions.Background;

/// <summary>
/// Report service
/// </summary>
public interface IReportService
{
    /// <summary>
    /// Generate report
    /// </summary>
    /// <param name="reportType"></param>
    /// <param name="department"></param>
    /// <returns></returns>
    Task<byte[]> GenerateReportAsync(string reportType, string department);

    /// <summary>
    /// Save report
    /// </summary>
    /// <param name="reportData"></param>
    /// <param name="fileName"></param>
    /// <returns></returns>
    Task SaveReportAsync(byte[] reportData, string fileName);
}
