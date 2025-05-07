using System.Collections.Generic;
using BaseApiReference.Entities;

namespace BaseApiReference.Metadata;

/// <summary>
/// Metadata for PasswordHistory
/// /summary
public class PasswordHistoryMetadata : ITableMetadata
{
    public string Name => "PasswordHistory";
    public string Schema => "dbo";

    public IReadOnlyDictionary<string, string> ColumnMappings => new Dictionary<string, string>
    {
        { nameof(PasswordHistory.UserId), "UserId" },
        { nameof(PasswordHistory.PasswordHash), "PasswordHash" },
        { nameof(PasswordHistory.CreatedAt), "CreatedAt" },
        { nameof(PasswordHistory.User), "User" }
    };
}