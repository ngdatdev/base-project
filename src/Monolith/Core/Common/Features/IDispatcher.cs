using System.Threading;
using System.Threading.Tasks;

namespace Common.Features;

/// <summary>
///     Defines an interface for a dispatch to send requests and receive corresponding responses.
/// </summary>
public interface IDispatcher
{
    /// <summary>
    ///     Sends a request and receives a response from the corresponding handler.
    /// </summary>
    /// <param name="request">
    ///     The type of the request. Must implement <see cref="IRequest{TResponse}"/>.
    /// </param>
    /// <param name="cancellationToken">
    ///     A token that is used to notify the system
    ///     to cancel the current operation when user stop
    ///     the request.
    /// </param>
    /// <returns>
    ///     A task with a response of type <typeparamref name="TResponse"/>.
    /// </returns>
    Task<TResponse> SendAsync<TResponse>(
        IRequest<TResponse> request,
        CancellationToken cancellationToken = default
    )
        where TResponse : class, IResponse;
}
