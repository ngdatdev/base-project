using System.Threading.Tasks;
using BaseApiReference.Abstractions.Background;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Background.Services;

/// <summary>
/// Dummy data sync service
/// </summary>
public class DataSyncService : IDataSyncService
{
    private readonly ILogger<DataSyncService> _logger;

    public DataSyncService(ILogger<DataSyncService> logger)
    {
        _logger = logger;
    }

    public async Task SyncExternalDataAsync()
    {
        _logger.LogInformation("Syncing external data...");
        await Task.Delay(3000);
        _logger.LogInformation("Data sync completed");
    }

    public async Task<bool> CheckDataIntegrityAsync()
    {
        await Task.Delay(1000);
        return true;
    }
}
