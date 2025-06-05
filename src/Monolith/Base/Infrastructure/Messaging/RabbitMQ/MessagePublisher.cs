using System;
using System.Text;
using System.Threading.Tasks;
using BaseApiReference.Abstractions.MessageBrokers.QueueBase;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Infrastructure.Messaging.RabbitMQ;

/// <summary>
/// RabbitMQ message publisher
/// </summary>
public class MessagePublisher : IMessagePublisher
{
    private readonly RabbitMQConnectionFactory _connectionFactory;
    private readonly ILogger<MessagePublisher> _logger;

    public MessagePublisher(
        RabbitMQConnectionFactory connectionFactory,
        ILogger<MessagePublisher> logger
    )
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public async Task PublishAsync<T>(
        T message,
        string exchange,
        string routingKey = "",
        MessageProperties properties = null
    )
        where T : class
    {
        using var channel = await _connectionFactory.CreateChannel();

        var messageBody = JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(messageBody);

        var basicProperties = new BasicProperties();
        SetBasicProperties(basicProperties, properties);

        try
        {
            await channel.BasicPublishAsync(
                exchange: exchange,
                routingKey: routingKey,
                basicProperties: basicProperties,
                mandatory: true,
                body: body
            );

            _logger.LogInformation(
                $"Message published to exchange '{exchange}' with routing key '{routingKey}'"
            );
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to publish message to exchange '{exchange}'");
            throw;
        }
    }

    public async Task PublishAsync<T>(
        T message,
        string queueName,
        MessageProperties properties = null
    )
        where T : class
    {
        using var channel = await _connectionFactory.CreateChannel();

        // Declare queue if not exists
        await channel.QueueDeclareAsync(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false
        );

        var messageBody = JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(messageBody);

        var basicProperties = new BasicProperties();
        SetBasicProperties(basicProperties, properties);

        try
        {
            await channel.BasicPublishAsync(
                exchange: "",
                routingKey: queueName,
                basicProperties: basicProperties,
                mandatory: true,
                body: body
            );

            _logger.LogInformation($"Message published to queue '{queueName}'");
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to publish message to queue '{queueName}'");
            throw;
        }
    }

    private void SetBasicProperties(IBasicProperties basicProperties, MessageProperties properties)
    {
        if (properties == null)
            return;

        basicProperties.Persistent = properties.Persistent;
        basicProperties.ContentType = properties.ContentType;
        basicProperties.ContentEncoding = properties.ContentEncoding;
        basicProperties.Priority = properties.Priority;

        if (!string.IsNullOrEmpty(properties.CorrelationId))
            basicProperties.CorrelationId = properties.CorrelationId;

        if (!string.IsNullOrEmpty(properties.ReplyTo))
            basicProperties.ReplyTo = properties.ReplyTo;

        if (properties.Expiration.HasValue)
            basicProperties.Expiration = properties.Expiration.Value.TotalMilliseconds.ToString();

        if (!string.IsNullOrEmpty(properties.MessageId))
            basicProperties.MessageId = properties.MessageId;

        if (properties.Timestamp.HasValue)
            basicProperties.Timestamp = new AmqpTimestamp(
                ((DateTimeOffset)properties.Timestamp.Value).ToUnixTimeSeconds()
            );

        if (!string.IsNullOrEmpty(properties.Type))
            basicProperties.Type = properties.Type;

        if (!string.IsNullOrEmpty(properties.UserId))
            basicProperties.UserId = properties.UserId;

        if (!string.IsNullOrEmpty(properties.AppId))
            basicProperties.AppId = properties.AppId;

        if (properties.Headers?.Count > 0)
            basicProperties.Headers = properties.Headers;
    }
}
