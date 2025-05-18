using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BaseApiReference.Abstractions.Tokens;

namespace Common.Applications.Tokens;

/// <summary>
/// OTP Generator
/// </summary>
internal sealed class OTPHandler : IOTPHandler
{
    public string Generate(int length)
    {
        const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

        StringBuilder builder = new();

        for (int time = default; time < length; time++)
        {
            builder.Append(
                value: Chars[index: RandomNumberGenerator.GetInt32(toExclusive: Chars.Length)]
            );
        }

        return builder.ToString();
    }
}
