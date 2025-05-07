using System.Collections.Generic;
using BaseApiReference.Entities;

namespace BaseApiReference.Metadata;

/// <summary>
/// Metadata for UserToken
/// </summary>
public class UserTokenMetadata : ITableMetadata
{
    public string Name => "UserToken";
    public string Schema => "dbo";

    public IReadOnlyDictionary<string, string> ColumnMappings =>
        new Dictionary<string, string>
        {
            { nameof(UserToken.UserId), "UserId" },
            { nameof(UserToken.LoginProvider), "LoginProvider" },
            { nameof(UserToken.Name), "Name" },
            { nameof(UserToken.Value), "Value" },
            { nameof(UserToken.ExpiredAt), "ExpiredAt" } // custom field
            // Navigation properties (optional, usually not included):
            // { nameof(UserToken.User), "User" }
        };
}
