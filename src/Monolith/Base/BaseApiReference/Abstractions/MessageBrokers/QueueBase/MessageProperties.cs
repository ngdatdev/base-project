using System;
using System.Collections.Generic;

namespace BaseApiReference.Abstractions.MessageBrokers.QueueBase;

/// <summary>
/// Represents message properties
/// </summary>
public class MessageProperties
{
    public bool Persistent { get; set; } = true;
    public string ContentType { get; set; } = "application/json";
    public string ContentEncoding { get; set; } = "utf-8";
    public IDictionary<string, object> Headers { get; set; } = new Dictionary<string, object>();
    public byte Priority { get; set; } = 0;
    public string CorrelationId { get; set; }
    public string ReplyTo { get; set; }
    public TimeSpan? Expiration { get; set; }
    public string MessageId { get; set; }
    public DateTime? Timestamp { get; set; }
    public string Type { get; set; }
    public string UserId { get; set; }
    public string AppId { get; set; }
}
