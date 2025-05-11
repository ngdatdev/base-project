using System;
using System.Globalization;

namespace Common.Helper;

/// <summary>
/// Decimal Util.
/// </summary>
public class DecimalUtil
{
    public static bool AreEqual(decimal a, decimal b, decimal epsilon = 0.0001m)
    {
        return Math.Abs(a - b) < epsilon;
    }

    public static bool AreEqual(double a, double b, double epsilon = 0.0001)
    {
        return Math.Abs(a - b) < epsilon;
    }

    public static decimal Round(decimal value, int digits = 2)
    {
        return Math.Round(value, digits, MidpointRounding.AwayFromZero);
    }

    public static double Round(double value, int digits = 2)
    {
        return Math.Round(value, digits, MidpointRounding.AwayFromZero);
    }

    public static decimal Ceil(decimal value)
    {
        return Math.Ceiling(value);
    }

    public static double Ceil(double value)
    {
        return Math.Ceiling(value);
    }

    public static decimal Floor(decimal value)
    {
        return Math.Floor(value);
    }

    public static double Floor(double value)
    {
        return Math.Floor(value);
    }

    public static string FormatCurrency(decimal value, string culture = "vi-VN")
    {
        return value.ToString("C", CultureInfo.CreateSpecificCulture(culture));
    }

    public static string FormatCurrency(double value, string culture = "vi-VN")
    {
        return value.ToString("C", CultureInfo.CreateSpecificCulture(culture));
    }

    public static bool IsValidDecimal(string input)
    {
        return decimal.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out _);
    }

    public static bool IsValidDouble(string input)
    {
        return double.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out _);
    }

    public static decimal ParseDecimal(string input, decimal defaultValue = 0)
    {
        return decimal.TryParse(
            input,
            NumberStyles.Any,
            CultureInfo.InvariantCulture,
            out var result
        )
            ? result
            : defaultValue;
    }

    public static double ParseDouble(string input, double defaultValue = 0)
    {
        return double.TryParse(
            input,
            NumberStyles.Any,
            CultureInfo.InvariantCulture,
            out var result
        )
            ? result
            : defaultValue;
    }

    public static string ToPercentage(decimal part, decimal total, int digits = 2)
    {
        if (total == 0)
            return "0%";
        decimal percent = (part / total) * 100;
        return $"{Math.Round(percent, digits)}%";
    }

    public static string ToPercentage(double part, double total, int digits = 2)
    {
        if (total == 0)
            return "0%";
        double percent = (part / total) * 100;
        return $"{Math.Round(percent, digits)}%";
    }
}
