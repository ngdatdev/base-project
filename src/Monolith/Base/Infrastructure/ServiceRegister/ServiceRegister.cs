using Infrastructure.ApiWeb;
using Infrastructure.Background;
using Infrastructure.Caching;
using Infrastructure.CSV;
using Infrastructure.Excel;
using Infrastructure.IdGenerator;
using Infrastructure.Notification.Email;
using Infrastructure.Notification.TwilioSMS;
using Infrastructure.ObjectStorage;
using Infrastructure.Payment.PayOs;
using Infrastructure.Payment.VietQr;
using Infrastructure.Payment.VNPay;
using Infrastructure.Persistence.PostgreSQL.Extentions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.ServiceRegister;

/// <summary>
/// Service Register.
/// </summary>
public static class ServiceRegister
{
    /// <summary>
    /// Register Infrastructure
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public static void RegisterInfrastructure(
        this IServiceCollection services,
        IConfigurationManager configuration
    )
    {
        services.AddApiWebConfig(configuration);
        services.AddIdGenerator();
        services.AddCaching(configuration, CacheType.Hybrid);
        services.AddDatabaseContextPool(configuration);
        services.AddAspNetCoreIdentity(configuration);
        services.AddCSVServices();
        services.AddExcelServices();
        services.AddMailKitServices(configuration);
        services.AddSMSServices(configuration);
        services.AddVietQRCodeService();
        services.AddStorageServices(configuration);
        // services.AddBackgroundServices(configuration);
        // services.AddPayOSService(configuration);
        // services.AddVNPayService(configuration);
    }
}
