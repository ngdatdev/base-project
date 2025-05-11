using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Common.Features;

/// <summary>
/// Implement of IDispatcher.
/// </summary>
public class Dispatcher : IDispatcher
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<Dispatcher> _logger;

    public Dispatcher(IServiceProvider serviceProvider, ILogger<Dispatcher> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task<TResponse> SendAsync<TResponse>(
        IRequest<TResponse> request,
        CancellationToken cancellationToken = default
    )
        where TResponse : class, IResponse
    {
        ArgumentNullException.ThrowIfNull(request);

        try
        {
            Type handlerType = typeof(IHandler<,>).MakeGenericType(
                request.GetType(),
                typeof(TResponse)
            );
            object handler = _serviceProvider.GetService(handlerType);

            MethodInfo handlerMethod =
                handlerType.GetMethod(nameof(IHandler<IRequest<TResponse>, TResponse>.HandlerAsync))
                ?? throw new InvalidOperationException(
                    $"Handler for '{handlerType.Name}' does not implement the 'HandlerAsync' method."
                );

            TResponse response =
                await (Task<TResponse>)handlerMethod.Invoke(handler, [request, cancellationToken])
                ?? throw new InvalidOperationException(
                    $"Handler for '{typeof(IRequest<TResponse>).Name}' returned null response."
                );

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error occurred while handling request of type {RequestType}",
                request.GetType()
            );
            throw;
        }
    }
}
