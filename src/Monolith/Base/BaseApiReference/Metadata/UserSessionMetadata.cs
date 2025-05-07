using System.Collections.Generic;
using BaseApiReference.Entities;

namespace BaseApiReference.Metadata;

/// <summary>
/// Metadata for UserSession
/// /summary
public class UserSessionMetadata : ITableMetadata
{
    public string Name => "UserSession";
    public string Schema => "dbo";

    public IReadOnlyDictionary<string, string> ColumnMappings => new Dictionary<string, string>
    {
        { nameof(UserSession.ExperiedDate), "ExperiedDate" },
        { nameof(UserSession.RefreshToken), "RefreshToken" },
        { nameof(UserSession.DeviderId), "DeviderId" },
        { nameof(UserSession.IpAddress), "IpAddress" },
        { nameof(UserSession.LastActive), "LastActive" },
        { nameof(UserSession.User), "User" }
    };
}