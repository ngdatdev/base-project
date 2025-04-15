using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Common.Helper;

public static class UrlUtil
{
    public static string BuildQuery(Dictionary<string, string> query)
    {
        return string.Join(
            "&",
            query.Select(kvp =>
                $"{HttpUtility.UrlEncode(kvp.Key)}={HttpUtility.UrlEncode(kvp.Value)}"
            )
        );
    }

    public static string UrlEncode(string text) => HttpUtility.UrlEncode(text);

    public static string UrlDecode(string encoded) => HttpUtility.UrlDecode(encoded);
}
