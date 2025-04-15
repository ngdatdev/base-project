using System;

namespace Common.Constants;

/// <summary>
/// Common constants.
/// </summary>
public static class Constant
{
    public const string BLANK = "";

    public const string SPACE = " ";

    public const string NEWLINE = "\n";

    public const string TAB = "\t";

    public const string CRLF = "\r\n";

    public const string COMMA = ",";

    public const string SEMICOLON = ";";

    public static DateTime DateTimeNow = DateTime.Now;

    public static DateTime DateTimeUtcNow = DateTime.UtcNow;

    public static DateTime MinTime = DateTime.MinValue;

    public static DateTime MaxTime = DateTime.MaxValue;
}
