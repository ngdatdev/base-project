namespace Infrastructure.Persistence.PostgreSQL.Common;

/// <summary>
/// Database constants.
/// </summary>
internal static class Constant
{
    public const string DatabaseSchema = "auths";

    public static class Collation
    {
        public const string CASE_INSENSITIVE = "case_insensitive";

        public const string LOCALE = "en-u-ks-primary";
    }

    public static class DatabaseType
    {
        public const string VARCHAR = "VARCHAR";

        public const string LONG = "BIGINT";

        public const string TIMESTAMPZ = "TIMESTAMP WITH TIME ZONE";
    }
}
