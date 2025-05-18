using System.Threading;
using System.Threading.Tasks;
using Common.Constants;
using Common.Features;
using Common.Filters;
using Common.HttpResponseMapper;
using Feature000.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Feature000.Logic;

/// <summary>
/// F000 Endpoint
/// </summary>
[Route("api/auth")]
[Tags(Constant.Module.AUTH)]
public sealed class F000Endpoint : ControllerBase
{
    private IDispatcher _dispatcher;

    public F000Endpoint(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    /// <summary>
    ///     Endpoint for user login.
    /// </summary>
    /// <param name="request">
    ///     Class contains user credentials.
    /// </param>
    /// <param name="cancellationToken">
    ///     Automatic initialized token for aborting current operation.
    /// </param>
    /// <returns>
    ///     App code.
    /// </returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST api/auth/log-in
    ///     {
    ///         "username": "string",
    ///         "password": "string",
    ///     }
    ///
    /// </remarks>
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet]
    [ServiceFilter(typeof(ValidationRequestFilter<F000Request>))]
    public async Task<IActionResult> ExecuteF000(
        F000Request request,
        CancellationToken cancellationToken
    )
    {
        var appResponse = await _dispatcher.SendAsync(request, cancellationToken);
        var httpResponse = appResponse.Map();
        return StatusCode(httpResponse.HttpCode, httpResponse);
    }
}
