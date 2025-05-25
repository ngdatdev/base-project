using BaseApiReference.Abstractions.ObjectStorage;
using CloudinaryDotNet;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.ObjectStorage;

/// <summary>
/// Extension methods for configuring storage services.
/// </summary>
internal static class StorageServicesExtension
{
    public static void AddStorageServices(
        this IServiceCollection services,
        IConfigurationManager configuration
    )
    {
        services.AddSingleton<IStorageImageHandler, StorageImageHandler>();

        services.AddSingleton(provider =>
        {
            var cloudinaryOption = configuration.GetSection("Cloudinary").Get<CloudinaryOption>();

            var account = new Account(
                cloudinaryOption.CloudName,
                cloudinaryOption.ApiKey,
                cloudinaryOption.ApiSecret
            );
            var cloudinary = new Cloudinary(account) { Api = { Secure = true, } };

            return cloudinary;
        });
    }
}
