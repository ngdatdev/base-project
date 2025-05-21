using System;

namespace Common.Helper;

/// <summary>
/// DateTime Util.
/// </summary>
public static class DateTimeUtil
{
    public static DateTime Now => DateTime.Now;

    public static DateTime UtcNow => DateTime.UtcNow;

    public static DateTimeOffset OffsetNow => DateTimeOffset.Now;

    public static DateTimeOffset OffsetUtcNow => DateTimeOffset.UtcNow;

    public static string ToReadableFormat(DateTime date) => date.ToString("dd/MM/yyyy HH:mm");

    public static DateTime StartOfWeek(DateTime dt) => dt.AddDays(-(int)dt.DayOfWeek);

    public static DateTime EndOfWeek(DateTime dt) => StartOfWeek(dt).AddDays(6);

    public static string ToIso8601(DateTime dt) => dt.ToString("yyyy-MM-ddTHH:mm:ssZ");

    public static int Age(DateTime dob) => (int)((DateTime.Now - dob).TotalDays / 365.25);
}
