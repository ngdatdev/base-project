using System.Threading.Tasks;

namespace BaseApiReference.Abstractions.Background;

/// <summary>
/// Data sync service
/// </summary>
public interface IDataSyncService
{
    /// <summary>
    /// Sync external data
    /// </summary>
    /// <returns></returns>
    Task SyncExternalDataAsync();

    /// <summary>
    /// Check data integrity
    /// </summary>
    /// <returns></returns>
    Task<bool> CheckDataIntegrityAsync();
}
