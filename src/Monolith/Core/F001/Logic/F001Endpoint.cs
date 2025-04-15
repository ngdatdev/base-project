using System.Threading;
using System.Threading.Tasks;
using Common.Features;
using Common.HttpResponseMapper;
using F001.Data;
using Microsoft.AspNetCore.Mvc;

namespace F001.Logic;

[Route("api/[controller]")]
public class F001Endpoint : ControllerBase
{
    private IDispatcher _dispatcher;

    public F001Endpoint(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    [HttpGet]
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
