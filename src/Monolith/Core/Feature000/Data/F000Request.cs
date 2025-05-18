using Common.Features;

namespace Feature000.Data;

/// <summary>
/// F000 request.
/// </summary>
public class F000Request : IRequest<F000Response>
{
    public string Username { get; set; }
    public string Password { get; set; }
}
