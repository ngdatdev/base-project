using System;
using System.IdentityModel.Tokens.Jwt;

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

    public static class ClaimType
    {
        public const string JTI = JwtRegisteredClaimNames.Jti;

        public const string EXP = JwtRegisteredClaimNames.Exp;

        public const string SUB = JwtRegisteredClaimNames.Sub;

        public static class PURPOSE
        {
            public const string Name = "purpose";

            public static class Value
            {
                public const string USER_PASSWORD_RESET = "user_password_reset";

                public const string USER_IN_APP = "user_in_app";
            }
        }
    }

    public static class Module
    {
        public const string AUTH = "Auth";
    }
}
