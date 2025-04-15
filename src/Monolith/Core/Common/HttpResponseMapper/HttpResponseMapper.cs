using Common.Features;

namespace Common.HttpResponseMapper;

/// <summary>
/// Manages HTTP responses.
/// </summary>
public static class HttpResponseMapper
{
    /// <summary>
    /// Maps an IResponse object to an ApiResponse.
    /// </summary>
    /// <param name="response">The response object to map.</param>
    /// <returns>Mapped ApiResponse.</returns>
    public static ApiResponse Map(this IResponse response)
    {
        return new ApiResponse
        {
            HttpCode = response.StatusCode,
            Message = response.Message,
            Body = response.Data ?? new(),
            DetailErrors = response.DetailErrors ?? []
        };
    }
}
