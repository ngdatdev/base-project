namespace BaseApiReference.Abstractions.Tokens;

/// <summary>
/// Interface for OTP Handler
/// </summary>
public interface IOTPHandler
{
    /// <summary>
    /// Generate OTP
    /// </summary>
    /// <param name="length"></param>
    /// <returns></returns>
    string Generate(int length);
}
