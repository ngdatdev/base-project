using System.Collections.Generic;
using BaseApiReference.Entities;

namespace BaseApiReference.Metadata;

/// <summary>
/// Metadata for User
/// /summary
public class UserMetadata : ITableMetadata
{
    public string Name => "User";
    public string Schema => "dbo";

    public IReadOnlyDictionary<string, string> ColumnMappings => new Dictionary<string, string>
    {
        { nameof(User.FullName), "FullName" },
        { nameof(User.Avatar), "Avatar" },
        { nameof(User.Status), "Status" },
        { nameof(User.LastLogin), "LastLogin" },
        { nameof(User.PreferedTwoFAMethod), "PreferedTwoFAMethod" },
        { nameof(User.CreatedAt), "CreatedAt" },
        { nameof(User.CreatedBy), "CreatedBy" },
        { nameof(User.UpdatedAt), "UpdatedAt" },
        { nameof(User.UpdatedBy), "UpdatedBy" },
        { nameof(User.RemovedAt), "RemovedAt" },
        { nameof(User.RemovedBy), "RemovedBy" }
    };
}