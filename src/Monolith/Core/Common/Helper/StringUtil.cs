using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Common.Helper;

/// <summary>
/// String Util.
/// </summary>
public static class StringUtil
{
    public static bool IsNullOrEmpty(string input)
    {
        return string.IsNullOrEmpty(input);
    }

    public static bool IsNullOrWhiteSpace(string input)
    {
        return string.IsNullOrWhiteSpace(input);
    }

    public static string CapitalizeWords(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return input;
        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input.ToLower());
    }

    public static string CapitalizeFirstLetter(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;
        return char.ToUpper(input[0]) + input.Substring(1);
    }

    public static string RemoveDiacritics(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return input;
        var normalized = input.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();
        foreach (var c in normalized)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
            {
                sb.Append(c);
            }
        }
        return sb.ToString().Normalize(NormalizationForm.FormC);
    }

    public static string Truncate(string input, int maxLength)
    {
        if (string.IsNullOrEmpty(input))
            return input;
        return input.Length <= maxLength ? input : input.Substring(0, maxLength) + "...";
    }

    public static string Reverse(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;
        char[] array = input.ToCharArray();
        Array.Reverse(array);
        return new string(array);
    }

    public static int WordCount(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return 0;
        return Regex.Matches(input.Trim(), @"\b\S+\b").Count;
    }

    public static string RemoveExtraSpaces(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;
        return Regex.Replace(input.Trim(), @"\s+", " ");
    }

    public static bool ContainsIgnoreCase(string source, string toCheck)
    {
        return source?.IndexOf(toCheck, StringComparison.OrdinalIgnoreCase) >= 0;
    }

    public static string ToSlug(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;
        string noDiacritics = RemoveDiacritics(input);
        string slug = Regex.Replace(noDiacritics.ToLower(), @"[^a-z0-9\s-]", "");
        slug = Regex.Replace(slug, @"\s+", "-").Trim('-');
        return slug;
    }

    public static bool IsValidEmail(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return false;

        return Regex.IsMatch(input, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);
    }

    public static string FromBase64(string base64)
    {
        if (string.IsNullOrEmpty(base64))
            return string.Empty;

        byte[] bytes = Convert.FromBase64String(base64);
        return Encoding.UTF8.GetString(bytes);
    }

    public static string ToBase64(string input)
    {
        if (string.IsNullOrEmpty(input))
            return string.Empty;

        byte[] bytes = Encoding.UTF8.GetBytes(input);
        return Convert.ToBase64String(bytes);
    }

    public static bool IsValidVietnamPhone(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return false;

        return Regex.IsMatch(input, @"^(0|\+84)(3[2-9]|5[6|8|9]|7[0|6-9]|8[1-5]|9[0-9])[0-9]{7}$");
    }

    public static string ExtractLetters(string input)
    {
        if (string.IsNullOrEmpty(input))
            return string.Empty;

        return Regex.Replace(input, @"[^a-zA-Z]", "");
    }

    public static bool IsValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out Uri result)
            && (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps);
    }

    public static List<string> SplitByLength(string input, int chunkSize)
    {
        var result = new List<string>();
        if (string.IsNullOrEmpty(input) || chunkSize <= 0)
            return result;

        for (int i = 0; i < input.Length; i += chunkSize)
        {
            result.Add(input.Substring(i, Math.Min(chunkSize, input.Length - i)));
        }

        return result;
    }

    public static bool EqualsIgnoreCase(string a, string b)
    {
        return string.Equals(a, b, StringComparison.OrdinalIgnoreCase);
    }

    public static bool EqualsIgnoreWhitespace(string a, string b)
    {
        return RemoveExtraSpaces(a) == RemoveExtraSpaces(b);
    }

    public static bool EqualsFullNormalize(string a, string b)
    {
        string normalize(string s) =>
            RemoveExtraSpaces(RemoveDiacritics(s ?? "").ToLowerInvariant());

        return normalize(a) == normalize(b);
    }

    public static bool EqualsIgnoreCaseAndWhitespace(string a, string b)
    {
        return string.Equals(
            RemoveExtraSpaces(a ?? "").ToLower(),
            RemoveExtraSpaces(b ?? "").ToLower(),
            StringComparison.OrdinalIgnoreCase
        );
    }
}
