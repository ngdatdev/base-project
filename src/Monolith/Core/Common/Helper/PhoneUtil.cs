using System.Text.RegularExpressions;

namespace Common.Helper;

/// <summary>
/// Phone Util.
/// </summary>
public static class PhoneUtil
{
    public static string Format(string phone) => Regex.Replace(phone, "[^0-9]", "");

    public static bool IsValid(string phone) => Regex.IsMatch(phone, @"^\d{9,11}$");

    public static string Mask(string phone) =>
        phone.Length > 4 ? new string('*', phone.Length - 4) + phone[^4..] : phone;
}
