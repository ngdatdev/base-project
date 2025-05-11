using System.Collections.Generic;

namespace Common.Features;

/// <summary>
/// Marker interface to represent a response
/// </summary>
public interface IResponse
{
    public int StatusCode { get; set; }

    public string Message { get; set; }

    public object Data { get; set; }

    public IEnumerable<string> DetailErrors { get; set; }
}
