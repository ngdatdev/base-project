using FluentValidation;

namespace Common.Features;

/// <summary>
/// Abstract for feature request validators.
/// </summary>
public abstract class Validator<TRequest, TResponse> : AbstractValidator<TRequest>
    where TRequest : class, IRequest<TResponse>
    where TResponse : class, IResponse { }
