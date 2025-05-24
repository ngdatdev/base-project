using BaseApiReference.Abstractions.Notification.Email;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Notification.Email;

/// <summary>
/// The MailKitServicesExtention class is used to register mailkit services.
/// </summary>
public static class MailKitServicesExtention
{
    public static void AddMailKitServices(
        this IServiceCollection services,
        IConfigurationManager configuration
    )
    {
        services.AddSingleton<ISendingMailHandler, GoogleSendingMailHandler>();
        services.AddSingleton(
            configuration
                .GetRequiredSection(key: "SmtpServer")
                .GetRequiredSection(key: "GoogleGmail")
                .Get<GoogleGmailSendingOption>()
        );
    }
}
