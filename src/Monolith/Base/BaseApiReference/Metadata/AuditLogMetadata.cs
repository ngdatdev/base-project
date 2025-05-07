using System.Collections.Generic;
using BaseApiReference.Entities;

namespace BaseApiReference.Metadata;

/// <summary>
/// Metadata for AuditLog
/// /summary
public class AuditLogMetadata : ITableMetadata
{
    public string Name => "AuditLog";
    public string Schema => "dbo";

    public IReadOnlyDictionary<string, string> ColumnMappings => new Dictionary<string, string>
    {
        { nameof(AuditLog.UserId), "UserId" },
        { nameof(AuditLog.Action), "Action" },
        { nameof(AuditLog.Data), "Data" },
        { nameof(AuditLog.CreatedAt), "CreatedAt" },
        { nameof(AuditLog.IpAddress), "IpAddress" },
        { nameof(AuditLog.User), "User" }
    };
}