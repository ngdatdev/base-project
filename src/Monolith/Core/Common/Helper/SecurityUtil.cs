using System;
using System.Security.Cryptography;
using System.Text;

namespace Common.Helper;

/// <summary>
/// Security Util.
/// </summary>
public static class SecurityUtil
{
    public static string Sha256(string input)
    {
        using var sha = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(input);
        var hash = sha.ComputeHash(bytes);
        return BitConverter.ToString(hash).Replace("-", "").ToLower();
    }

    public static string ToBase64(string input) =>
        Convert.ToBase64String(Encoding.UTF8.GetBytes(input));

    public static string FromBase64(string base64) =>
        Encoding.UTF8.GetString(Convert.FromBase64String(base64));
}
