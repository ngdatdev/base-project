using System;
using System.Collections.Generic;

namespace BaseApiReference.Abstractions.MessageBrokers.QueueBase;

/// <summary>
/// Represents message context
/// </summary>
public class MessageContext
{
    public string MessageId { get; set; }
    public string CorrelationId { get; set; }
    public DateTime Timestamp { get; set; }
    public string Exchange { get; set; }
    public string RoutingKey { get; set; }
    public string QueueName { get; set; }
    public ulong DeliveryTag { get; set; }
    public bool Redelivered { get; set; }
    public IDictionary<string, object> Headers { get; set; }
}
