using System;
using System.Threading;
using System.Threading.Tasks;
using BaseApiReference.Abstractions.ObjectStorage;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.ObjectStorage;

/// <summary>
/// Implementation of cloudinary storage handler.
/// </summary>
public sealed class StorageImageHandler : IStorageImageHandler
{
    private readonly Cloudinary _cloudinary;

    public StorageImageHandler(Cloudinary cloudinary)
    {
        _cloudinary = cloudinary;
    }

    public async Task<string> UploadPhotoAsync(IFormFile image, CancellationToken cancellationToken)
    {
        if (image == null || image.Length == 0)
        {
            return string.Empty;
        }
        using var stream = image.OpenReadStream();

        var uploadParams = new ImageUploadParams()
        {
            File = new FileDescription() { FileName = Guid.NewGuid().ToString(), Stream = stream },
            Overwrite = true
        };

        try
        {
            var uploadResult = await _cloudinary.UploadAsync(uploadParams, cancellationToken);
            if (uploadResult.Error != null)
            {
                return string.Empty;
            }

            return uploadResult.Url.ToString();
        }
        catch
        {
            return string.Empty;
        }
    }

    public async Task<bool> DeletePhotoAsync(string imageUrl)
    {
        if (string.IsNullOrEmpty(imageUrl))
        {
            return false;
        }

        try
        {
            var publicId = GetPublicId(imageUrl);
            var deleteParams = new DeletionParams(publicId)
            {
                ResourceType = CloudinaryDotNet.Actions.ResourceType.Image
            };

            var result = await _cloudinary.DestroyAsync(parameters: deleteParams);

            return result.Result == "ok";
        }
        catch
        {
            return false;
        }
    }

    private string GetPublicId(string imageUrl)
    {
        var uri = new Uri(imageUrl);
        return uri.Segments[uri.Segments.Length - 1].Split('.')[0];
    }
}
