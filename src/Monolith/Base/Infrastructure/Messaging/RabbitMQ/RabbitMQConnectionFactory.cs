using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Infrastructure.Messaging.RabbitMQ;

/// <summary>
/// RabbitMQ connection factory
/// </summary>
public class RabbitMQConnectionFactory
{
    private readonly RabbitMQOption _config;
    private readonly ILogger<RabbitMQConnectionFactory> _logger;
    private IConnection _connection;

    public RabbitMQConnectionFactory(
        IOptions<RabbitMQOption> config,
        ILogger<RabbitMQConnectionFactory> logger
    )
    {
        _config = config.Value;
        _logger = logger;
    }

    public async Task<IConnection> CreateConnection()
    {
        if (_connection != null && _connection.IsOpen)
            return _connection;

        if (_connection != null && _connection.IsOpen)
            return _connection;

        var factory = new ConnectionFactory
        {
            HostName = _config.HostName,
            Port = _config.Port,
            UserName = _config.UserName,
            Password = _config.Password,
            VirtualHost = _config.VirtualHost,
            AutomaticRecoveryEnabled = _config.AutomaticRecoveryEnabled,
            NetworkRecoveryInterval = _config.NetworkRecoveryInterval,
            RequestedHeartbeat = _config.RequestedHeartbeat
        };

        _connection = await factory.CreateConnectionAsync();

        _logger.LogInformation("RabbitMQ connection established");

        return _connection;
    }

    public async Task<IChannel> CreateChannel()
    {
        var connection = await CreateConnection();
        return await connection.CreateChannelAsync();
    }

    public void Dispose()
    {
        _connection?.Dispose();
    }
}
