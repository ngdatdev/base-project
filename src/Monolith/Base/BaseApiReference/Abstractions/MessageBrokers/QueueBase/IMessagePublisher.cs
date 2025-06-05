using System.Threading.Tasks;

namespace BaseApiReference.Abstractions.MessageBrokers.QueueBase;

/// <summary>
/// Represents a message publisher
/// </summary>
public interface IMessagePublisher
{
    /// <summary>
    /// Publishes a message
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="message"></param>
    /// <param name="exchange"></param>
    /// <param name="routingKey"></param>
    /// <param name="properties"></param>
    /// <returns></returns>
    Task PublishAsync<T>(
        T message,
        string exchange,
        string routingKey = "",
        MessageProperties properties = null
    )
        where T : class;

    /// <summary>
    /// Publishes a message async
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="message"></param>
    /// <param name="queueName"></param>
    /// <param name="properties"></param>
    /// <returns></returns>
    Task PublishAsync<T>(T message, string queueName, MessageProperties properties = null)
        where T : class;
}
