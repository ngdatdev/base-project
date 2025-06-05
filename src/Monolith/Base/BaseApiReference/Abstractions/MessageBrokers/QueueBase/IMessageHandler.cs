using System.Threading.Tasks;

namespace BaseApiReference.Abstractions.MessageBrokers.QueueBase;

/// <summary>
/// Represents a message handler
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IMessageHandler<T>
    where T : class
{
    /// <summary>
    /// Handles a message
    /// </summary>
    /// <param name="message"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    Task HandleAsync(T message, MessageContext context);
}
