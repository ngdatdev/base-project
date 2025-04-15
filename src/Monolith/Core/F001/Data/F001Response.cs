using System.Collections.Generic;
using Common.Features;

namespace F001.Data;

/// <summary>
/// F001 response.
/// </summary>
public class F001Response : IResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public object Data { get; set; } = new ResponseBody();
    public IEnumerable<string> DetailErrors { get; set; }
}

public class ResponseBody
{
    public string FullName { get; set; }
    public long GeneratorId { get; set; }
}
