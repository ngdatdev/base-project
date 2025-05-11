using System.Collections.Generic;
using BaseApiReference.Entities;

namespace BaseApiReference.Metadata;

/// <summary>
/// Metadata for RateLimit
/// /summary
public class RateLimitMetadata : ITableMetadata
{
    public string Name => "RateLimit";
    public string Schema => "dbo";

    public IReadOnlyDictionary<string, string> ColumnMappings =>
        new Dictionary<string, string>
        {
            { nameof(RateLimit.UserId), "UserId" },
            { nameof(RateLimit.IpAddress), "IpAddress" },
            { nameof(RateLimit.Endpoint), "Endpoint" },
            { nameof(RateLimit.Count), "Count" },
            { nameof(RateLimit.PeriodStart), "PeriodStart" },
            { nameof(RateLimit.User), "User" },
        };
}
