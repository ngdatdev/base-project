using Common.Features;
using FluentValidation;

namespace Feature000.Data;

/// <summary>
/// F000 validator.
/// </summary>
public class F000Validator : Validator<F000Request, F000Response>
{
    public F000Validator()
    {
        RuleFor(expression: request => request.Username).NotEmpty().MinimumLength(minimumLength: 5);

        RuleFor(expression: request => request.Password).NotEmpty().MinimumLength(minimumLength: 5);
    }
}
