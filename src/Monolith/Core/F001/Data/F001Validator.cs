using Common.Features;
using FluentValidation;

namespace F001.Data;

/// <summary>
/// F001 validator.
/// </summary>
public class F001Validator : Validator<F001Request, F001Response>
{
    public F001Validator()
    {
        RuleFor(expression: request => request.Name)
            .NotEmpty()
            .MaximumLength(maximumLength: 100)
            .MinimumLength(minimumLength: 10);
    }
}
