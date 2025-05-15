using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Common.HttpResponseMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Common.Filters;

/// <summary>
/// ValidationFilter is responsible for validating incoming requests.
/// </summary>
public class ValidationRequestFilter<TRequest> : IAsyncActionFilter
{
    private readonly IValidator<TRequest> _validator;

    public ValidationRequestFilter(IValidator<TRequest> validator)
    {
        _validator = validator;
    }

    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next
    )
    {
        var argument = context.ActionArguments.SingleOrDefault(arg => arg.Value is TRequest);
        if (argument.Value is not TRequest model)
        {
            await next();
            return;
        }

        var validationResult = await _validator.ValidateAsync(model);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(error => error.ErrorMessage).ToArray();
            SetErrorResponse(context.HttpContext, errors);
            context.Result = new BadRequestObjectResult(errors);
            return;
        }

        await next();
    }

    private static void SetErrorResponse(HttpContext httpContext, IEnumerable<string> errorMessage)
    {
        httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        var response = new ApiResponse
        {
            Message = AppCodes.VALIDATION_FAILED,
            DetailErrors = errorMessage
        };
        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        httpContext.Response.WriteAsJsonAsync(response);
    }
}
