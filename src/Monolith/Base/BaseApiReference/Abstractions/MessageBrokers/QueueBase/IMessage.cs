using System;

namespace BaseApiReference.Abstractions.MessageBrokers.QueueBase;

/// <summary>
/// Represents a message
/// </summary>
public interface IMessage
{
    string MessageId { get; set; }
    DateTime Timestamp { get; set; }
    string CorrelationId { get; set; }
}
