using System;

namespace Infrastructure.Messaging.RabbitMQ;

/// <summary>
/// RabbitMQ option
/// </summary>
public class RabbitMQOption
{
    public string HostName { get; set; } = "localhost";
    public int Port { get; set; } = 5672;
    public string UserName { get; set; } = "guest";
    public string Password { get; set; } = "guest";
    public string VirtualHost { get; set; } = "/";
    public TimeSpan ConnectionTimeout { get; set; } = TimeSpan.FromSeconds(30);
    public TimeSpan NetworkRecoveryInterval { get; set; } = TimeSpan.FromSeconds(10);
    public bool AutomaticRecoveryEnabled { get; set; } = true;
    public TimeSpan RequestedHeartbeat { get; set; } = TimeSpan.FromSeconds(10);
}
