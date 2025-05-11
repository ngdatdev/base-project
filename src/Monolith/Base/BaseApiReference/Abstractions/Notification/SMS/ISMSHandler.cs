using System.Threading.Tasks;

namespace BaseApiReference.Abstractions.Notification.SMS;

/// <summary>
/// Interface for SMS Handler
/// </summary>
public interface ISMSHandler
{
    /// <summary>
    /// Send SMS Notifications.
    /// </summary>
    /// <param name="to"></param>
    /// <param name="body"></param>
    /// <returns></returns>
    Task<bool> SendNotification(string to, string body);
}
