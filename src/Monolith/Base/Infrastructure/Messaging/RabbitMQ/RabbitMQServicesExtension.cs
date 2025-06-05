using BaseApiReference.Abstractions.MessageBrokers.QueueBase;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Messaging.RabbitMQ;

/// <summary>
/// RabbitMQ services
/// </summary>
internal static class RabbitMQServicesExtension
{
    public static void AddRabbitMQs(
        this IServiceCollection services,
        IConfigurationManager configuration
    )
    {
        services.Configure<RabbitMQOption>(configuration.GetSection("RabbitMQ"));
        services.AddSingleton<RabbitMQConnectionFactory>();
        services.AddSingleton<IMessageConsumer, MessageConsumer>();
        services.AddSingleton<IMessagePublisher, MessagePublisher>();
    }
}
