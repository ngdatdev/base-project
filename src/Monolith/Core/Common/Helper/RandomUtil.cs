using System;
using System.Linq;

namespace Common.Helper;

public static class RandomUtil
{
    private static readonly Random _rand = new();

    public static int RandomInt(int min, int max) => _rand.Next(min, max);

    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(
            Enumerable.Repeat(chars, length).Select(s => s[_rand.Next(s.Length)]).ToArray()
        );
    }
}
