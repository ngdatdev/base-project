using System;
using System.Threading.Tasks;
using Common.HttpResponseMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Middleware;

/// <summary>
///     Global Exception Handler
/// </summary>
public sealed class GlobalExceptionHandler
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly RequestDelegate _next;

    public GlobalExceptionHandler(RequestDelegate next, IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception exception)
        {
            if (!httpContext.Response.HasStarted)
            {
                httpContext.Response.Clear();
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

                await httpContext.Response.WriteAsJsonAsync(
                    value: new ApiResponse
                    {
                        Message = AppCodes.INTERNAL_SERVER_ERROR,
                        DetailErrors = ["Server has encountered an error !", exception.Message]
                    }
                );

                await httpContext.Response.CompleteAsync();
            }
        }
    }
}
