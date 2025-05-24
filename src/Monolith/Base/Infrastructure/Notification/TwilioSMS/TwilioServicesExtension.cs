using BaseApiReference.Abstractions.Notification.SMS;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Notification.TwilioSMS;

/// <summary>
/// Twilio SMS Extension
/// </summary>
internal static class TwilioServicesExtension
{
    public static void AddSMSServices(
        this IServiceCollection services,
        IConfigurationManager configurationManager
    )
    {
        services.AddSingleton(
            configurationManager.GetRequiredSection(key: "TwilioSMS").Get<TwilioSMSOption>()
        );

        services.AddScoped<ISMSHandler, TwilioSMSHandler>();
    }
}
