using System.Collections.Generic;
using BaseApiReference.Entities;

namespace BaseApiReference.Metadata;

/// <summary>
/// Metadata for BackupCode
/// /summary
public class BackupCodeMetadata : ITableMetadata
{
    public string Name => "BackupCode";
    public string Schema => "dbo";

    public IReadOnlyDictionary<string, string> ColumnMappings => new Dictionary<string, string>
    {
        { nameof(BackupCode.Code), "Code" },
        { nameof(BackupCode.IsUsed), "IsUsed" },
        { nameof(BackupCode.User), "User" },
        { nameof(BackupCode.UserId), "UserId" }
    };
}