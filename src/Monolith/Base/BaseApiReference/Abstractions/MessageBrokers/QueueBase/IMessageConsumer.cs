using System.Threading.Tasks;

namespace BaseApiReference.Abstractions.MessageBrokers.QueueBase;

/// <summary>
/// Represents a message consumer
/// </summary>
public interface IMessageConsumer
{
    /// <summary>
    /// Start consuming
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="queueName"></param>
    /// <param name="handler"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    Task StartConsumingAsync<T>(
        string queueName,
        IMessageHandler<T> handler,
        ConsumerOptions options = null
    )
        where T : class;

    /// <summary>
    /// Start consuming async
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="exchange"></param>
    /// <param name="routingKey"></param>
    /// <param name="handler"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    Task StartConsumingAsync<T>(
        string exchange,
        string routingKey,
        IMessageHandler<T> handler,
        ConsumerOptions options = null
    )
        where T : class;
    void StopConsuming();
}
