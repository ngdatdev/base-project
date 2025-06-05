using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BaseApiReference.Abstractions.MessageBrokers.QueueBase;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Infrastructure.Messaging.RabbitMQ;

/// <summary>
/// Consumes messages from a RabbitMQ queue
/// </summary>
public class MessageConsumer : IMessageConsumer, IDisposable
{
    private readonly RabbitMQConnectionFactory _connectionFactory;
    private readonly ILogger<MessageConsumer> _logger;
    private readonly List<IChannel> _channels = new List<IChannel>();
    private readonly List<AsyncEventingBasicConsumer> _consumers =
        new List<AsyncEventingBasicConsumer>();

    public MessageConsumer(
        RabbitMQConnectionFactory connectionFactory,
        ILogger<MessageConsumer> logger
    )
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public async Task StartConsumingAsync<T>(
        string queueName,
        IMessageHandler<T> handler,
        ConsumerOptions options = null
    )
        where T : class
    {
        options ??= new ConsumerOptions();
        var channel = await _connectionFactory.CreateChannel();
        _channels.Add(channel);

        var queueArgs = new Dictionary<string, object>();
        if (!string.IsNullOrEmpty(options.DeadLetterExchange))
        {
            queueArgs["x-dead-letter-exchange"] = options.DeadLetterExchange;
            if (!string.IsNullOrEmpty(options.DeadLetterRoutingKey))
                queueArgs["x-dead-letter-routing-key"] = options.DeadLetterRoutingKey;
        }

        await channel.QueueDeclareAsync(
            queue: queueName,
            durable: options.Durable,
            exclusive: options.Exclusive,
            autoDelete: options.AutoDelete,
            arguments: queueArgs
        );

        var consumer = new AsyncEventingBasicConsumer(channel);
        _consumers.Add(consumer);

        consumer.ReceivedAsync += async (model, ea) =>
        {
            await ProcessMessage(channel, ea, handler, options);
        };

        await channel.BasicConsumeAsync(
            queue: queueName,
            autoAck: options.AutoAck,
            consumer: consumer
        );
        _logger.LogInformation($"Started consuming messages from queue '{queueName}'");

        await Task.CompletedTask;
    }

    public async Task StartConsumingAsync<T>(
        string exchange,
        string routingKey,
        IMessageHandler<T> handler,
        ConsumerOptions options = null
    )
        where T : class
    {
        options ??= new ConsumerOptions();
        var channel = await _connectionFactory.CreateChannel();
        _channels.Add(channel);

        // Configure channel
        //channel.BasicQos(prefetchSize: 0, prefetchCount: options.PrefetchCount, global: false);

        await channel.ExchangeDeclareAsync(
            exchange: exchange,
            type: ExchangeType.Direct,
            durable: true
        );

        var queueName = channel.QueueDeclareAsync().Result.QueueName;
        await channel.QueueBindAsync(queue: queueName, exchange: exchange, routingKey: routingKey);

        var consumer = new AsyncEventingBasicConsumer(channel);
        _consumers.Add(consumer);

        consumer.ReceivedAsync += async (model, ea) =>
        {
            await ProcessMessage(channel, ea, handler, options);
        };

        await channel.BasicConsumeAsync(
            queue: queueName,
            autoAck: options.AutoAck,
            consumer: consumer
        );
        _logger.LogInformation(
            $"Started consuming messages from exchange '{exchange}' with routing key '{routingKey}'"
        );

        await Task.CompletedTask;
    }

    private async Task ProcessMessage<T>(
        IChannel channel,
        BasicDeliverEventArgs ea,
        IMessageHandler<T> handler,
        ConsumerOptions options
    )
        where T : class
    {
        var body = ea.Body.ToArray();
        var messageJson = Encoding.UTF8.GetString(body);

        try
        {
            var message = JsonConvert.DeserializeObject<T>(messageJson);
            var context = new MessageContext
            {
                MessageId = ea.BasicProperties?.MessageId,
                CorrelationId = ea.BasicProperties?.CorrelationId,
                Timestamp =
                    ea.BasicProperties?.Timestamp != null
                        ? DateTimeOffset
                            .FromUnixTimeSeconds(ea.BasicProperties.Timestamp.UnixTime)
                            .DateTime
                        : DateTime.UtcNow,
                Exchange = ea.Exchange,
                RoutingKey = ea.RoutingKey,
                DeliveryTag = ea.DeliveryTag,
                Redelivered = ea.Redelivered,
                Headers = ea.BasicProperties?.Headers
            };

            await handler.HandleAsync(message, context);

            if (!options.AutoAck)
            {
                await channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
            }

            _logger.LogInformation(
                $"Message processed successfully. DeliveryTag: {ea.DeliveryTag}"
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error processing message. DeliveryTag: {ea.DeliveryTag}");

            if (!options.AutoAck)
            {
                var retryCount = GetRetryCount(ea.BasicProperties?.Headers);
                if (retryCount < options.RetryCount)
                {
                    await Task.Delay(options.RetryDelay);
                    await channel.BasicNackAsync(
                        deliveryTag: ea.DeliveryTag,
                        multiple: false,
                        requeue: true
                    );
                }
                else
                {
                    await channel.BasicNackAsync(
                        deliveryTag: ea.DeliveryTag,
                        multiple: false,
                        requeue: false
                    );
                }
            }
        }
    }

    private int GetRetryCount(IDictionary<string, object> headers)
    {
        if (headers != null && headers.TryGetValue("x-retry-count", out var retryCountObj))
        {
            return Convert.ToInt32(retryCountObj);
        }
        return 0;
    }

    public void StopConsuming()
    {
        foreach (var channel in _channels)
        {
            channel?.Dispose();
        }
        _channels.Clear();
        _consumers.Clear();
        _logger.LogInformation("Stopped consuming messages");
    }

    public void Dispose()
    {
        StopConsuming();
    }
}
