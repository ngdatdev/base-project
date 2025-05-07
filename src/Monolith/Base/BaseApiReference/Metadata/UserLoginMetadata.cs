using System.Collections.Generic;
using BaseApiReference.Entities;

namespace BaseApiReference.Metadata;

/// <summary>
/// Metadata for UserLogin
/// </summary>
public class UserLoginMetadata : ITableMetadata
{
    public string Name => "UserLogin";
    public string Schema => "dbo";

    public IReadOnlyDictionary<string, string> ColumnMappings =>
        new Dictionary<string, string>
        {
            { nameof(UserLogin.LoginProvider), "LoginProvider" },
            { nameof(UserLogin.ProviderKey), "ProviderKey" },
            { nameof(UserLogin.ProviderDisplayName), "ProviderDisplayName" },
            { nameof(UserLogin.UserId), "UserId" }
        };
}
