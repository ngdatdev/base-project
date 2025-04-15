using Common.Features;

namespace F001.Data;

/// <summary>
/// F001 request.
/// </summary>
public class F001Request : IRequest<F001Response>
{
    public string Name { get; set; }
}
