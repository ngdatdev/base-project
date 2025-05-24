using BaseApiReference.Abstractions.Payments;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Payment.VNPay;

/// <summary>
/// Vnpay services extension
/// </summary>
internal static class VNPayServicesExtension
{
    public static void AddVNPayService(
        this IServiceCollection services,
        IConfigurationManager configuration
    )
    {
        services.AddSingleton(configuration.GetRequiredSection(key: "VNPay").Get<VNPayOption>());

        services.AddSingleton<IPaymentHandler, VNPayHandler>();
    }
}
