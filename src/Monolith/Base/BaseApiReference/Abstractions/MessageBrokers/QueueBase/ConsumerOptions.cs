using System;

namespace BaseApiReference.Abstractions.MessageBrokers.QueueBase;

/// <summary>
/// Represents consumer options
/// </summary>
public class ConsumerOptions
{
    public bool AutoAck { get; set; } = false;
    public ushort PrefetchCount { get; set; } = 1;
    public bool Durable { get; set; } = true;
    public bool Exclusive { get; set; } = false;
    public bool AutoDelete { get; set; } = false;
    public int RetryCount { get; set; } = 3;
    public TimeSpan RetryDelay { get; set; } = TimeSpan.FromSeconds(5);
    public string DeadLetterExchange { get; set; }
    public string DeadLetterRoutingKey { get; set; }
}
