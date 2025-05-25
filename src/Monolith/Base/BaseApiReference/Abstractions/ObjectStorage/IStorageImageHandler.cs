using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BaseApiReference.Abstractions.ObjectStorage;

/// <summary>
/// Represent the handler for image storage service.
/// </summary>
public interface IStorageImageHandler
{
    /// <summary>
    /// Handles the upload photo async with image.
    /// </summary>
    /// <param name="formFile">
    /// The request contain image file.
    ///  </param>
    /// <param name="cancellationToken">
    /// A token that is used to notify the system
    /// to cancel the current operation when user stop
    /// the request.
    /// </param>
    /// <returns>
    /// The response boolean.
    ///</returns>
    Task<string> UploadPhotoAsync(IFormFile formFile, CancellationToken cancellationToken);

    /// <summary>
    /// Handles the delete photo async with image.
    /// </summary>
    /// <param name="imageUrl">
    /// The request contain image file.
    /// </param>
    /// <returns>
    /// The response boolean.
    ///</returns>
    Task<bool> DeletePhotoAsync(string imageUrl);
}
