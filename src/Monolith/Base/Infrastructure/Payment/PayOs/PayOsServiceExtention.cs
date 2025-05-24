using BaseApiReference.Abstractions.Payments;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Net.payOS;

namespace Infrastructure.Payment.PayOs;

/// <summary>
/// PayOS service extention
/// </summary>
internal static class PayOsServiceExtention
{
    public static void ConfigPayOSService(
        this IServiceCollection services,
        IConfigurationManager configuration
    )
    {
        var option = configuration.GetRequiredSection(key: "PayOS").Get<PayOSOption>();

        services.AddSingleton<IPaymentHandler, PayOSHandler>();

        services.AddSingleton(provider =>
        {
            return new PayOS(clientId: option.ClientId, option.ApiKey, option.ChecksumKey);
        });
    }
}
