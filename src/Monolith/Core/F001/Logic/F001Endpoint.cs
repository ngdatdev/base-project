using System.Threading;
using System.Threading.Tasks;
using Common.Features;
using Common.Filters;
using Common.HttpResponseMapper;
using F001.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace F001.Logic;

/// <summary>
/// F001 Endpoint
/// </summary>
[Route("api/[controller]")]
public class F001Endpoint : ControllerBase
{
    private IDispatcher _dispatcher;

    public F001Endpoint(IDispatcher dispatcher)
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
    ///     POST api/auth/sign-in
    ///     {
    ///         "username": "string",
    ///         "password": "string",
    ///         "rememberMe": true
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
    [ServiceFilter(typeof(ValidationRequestFilter<F001Request>))]
    public async Task<IActionResult> GetHelloWorld(
        F001Request request,
        CancellationToken cancellationToken
    )
    {
        var helloWorld = await _dispatcher.SendAsync(request, cancellationToken);
        var result = helloWorld.Map();
        return StatusCode(result.HttpCode, result);
    }
}
