using System.Collections.Generic;

namespace BaseApiReference.Abstractions.MessageBrokers.QueueBase;

/// <summary>
/// Represents queue manager
/// </summary>
public interface IQueueManager
{
    object DeclareQueue(
        string queueName,
        bool durable = true,
        bool exclusive = false,
        bool autoDelete = false,
        IDictionary<string, object> arguments = null
    );
    uint DeleteQueue(string queueName, bool ifUnused = false, bool ifEmpty = false);
    uint PurgeQueue(string queueName);
    uint GetMessageCount(string queueName);
    uint GetConsumerCount(string queueName);
}
