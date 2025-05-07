using System.Collections.Generic;
using BaseApiReference.Entities;

namespace BaseApiReference.Metadata;

/// <summary>
/// Metadata for UserClaim
/// </summary>
public class UserClaimMetadata : ITableMetadata
{
    public string Name => "UserClaim";
    public string Schema => "dbo";

    public IReadOnlyDictionary<string, string> ColumnMappings =>
        new Dictionary<string, string>
        {
            { nameof(UserClaim.Id), "Id" },
            { nameof(UserClaim.UserId), "UserId" },
            { nameof(UserClaim.ClaimType), "ClaimType" },
            { nameof(UserClaim.ClaimValue), "ClaimValue" }
        };
}
