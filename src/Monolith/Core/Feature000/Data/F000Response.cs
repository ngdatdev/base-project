using System.Collections.Generic;
using Common.Features;

namespace Feature000.Data;

/// <summary>
/// F000 response.
/// </summary>
public class F000Response : IResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public object Data { get; set; } = new ResponseBody();
    public IEnumerable<string> DetailErrors { get; set; }
}

public class ResponseBody
{
    public string AccessToken { get; init; }

    public string RefreshToken { get; init; }

    public UserCredential User { get; init; }

    public sealed class UserCredential
    {
        public string Email { get; init; }

        public string AvatarUrl { get; init; }

        public string FullName { get; init; }
    }
}
