using System.Collections.Generic;

namespace BaseApiReference.Abstractions.MessageBrokers.QueueBase;

/// <summary>
/// Represents exchange manager
/// </summary>
public interface IExchangeManager
{
    /// <summary>
    /// Declares exchange
    /// </summary>
    /// <param name="exchangeName"></param>
    /// <param name="exchangeType"></param>
    /// <param name="durable"></param>
    /// <param name="autoDelete"></param>
    /// <param name="arguments"></param>
    void DeclareExchange(
        string exchangeName,
        string exchangeType,
        bool durable = true,
        bool autoDelete = false,
        IDictionary<string, object> arguments = null
    );

    /// <summary>
    /// Deletes exchange
    /// </summary>
    /// <param name="exchangeName"></param>
    /// <param name="ifUnused"></param>
    void DeleteExchange(string exchangeName, bool ifUnused = false);

    /// <summary>
    /// Binds queue
    /// </summary>
    /// <param name="queueName"></param>
    /// <param name="exchangeName"></param>
    /// <param name="routingKey"></param>
    void BindQueue(string queueName, string exchangeName, string routingKey = "");

    /// <summary>
    /// Unbinds queue
    /// </summary>
    /// <param name="queueName"></param>
    /// <param name="exchangeName"></param>
    /// <param name="routingKey"></param>
    void UnbindQueue(string queueName, string exchangeName, string routingKey = "");
}
