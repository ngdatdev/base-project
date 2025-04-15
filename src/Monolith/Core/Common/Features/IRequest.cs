namespace Common.Features;

/// <summary>
///     Marker interface to represent a request with a response
/// </summary>
public interface IRequest<out TResponse>
    where TResponse : class, IResponse { }
