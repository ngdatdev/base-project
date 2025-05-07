using System.Collections.Generic;
using BaseApiReference.Entities;

namespace BaseApiReference.Metadata;

/// <summary>
/// Metadata for UserTwoFactor
/// /summary
public class UserTwoFactorMetadata : ITableMetadata
{
    public string Name => "UserTwoFactor";
    public string Schema => "dbo";

    public IReadOnlyDictionary<string, string> ColumnMappings => new Dictionary<string, string>
    {
        { nameof(UserTwoFactor.SecretKey), "SecretKey" },
        { nameof(UserTwoFactor.TwoFAMethod), "TwoFAMethod" },
        { nameof(UserTwoFactor.IsEnabled), "IsEnabled" },
        { nameof(UserTwoFactor.User), "User" },
        { nameof(UserTwoFactor.UserId), "UserId" }
    };
}