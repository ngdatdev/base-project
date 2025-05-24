using System.Threading.Tasks;
using BaseApiReference.Abstractions.Payments;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Payment.VietQr;

/// <summary>
/// QRServicesExtension
/// </summary>
internal static class QRServicesExtension
{
    public static void AddVietQRCodeService(this IServiceCollection services)
    {
        services.AddSingleton<IQRCodeHandler, VietQRCodeHandler>();
    }
}
