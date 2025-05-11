using System;
using System.Collections.Generic;

namespace Common.Helper;

/// <summary>
/// Enum Util.
/// </summary>
public static class EnumUtil
{
    public static string GetName<T>(T enumVal)
        where T : Enum => Enum.GetName(typeof(T), enumVal) ?? string.Empty;

    public static IEnumerable<string> GetNames<T>()
        where T : Enum => Enum.GetNames(typeof(T));

    public static T Parse<T>(string value, bool ignoreCase = true)
        where T : Enum => (T)Enum.Parse(typeof(T), value, ignoreCase);
}
