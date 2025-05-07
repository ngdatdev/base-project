using System.Collections.Generic;
using BaseApiReference.Entities;

namespace BaseApiReference.Metadata;

/// <summary>
/// Metadata for RoleClaim
/// </summary>
public class RoleClaimMetadata : ITableMetadata
{
    public string Name => "RoleClaim";
    public string Schema => "dbo";

    public IReadOnlyDictionary<string, string> ColumnMappings =>
        new Dictionary<string, string>
        {
            { nameof(RoleClaim.Id), "Id" },
            { nameof(RoleClaim.RoleId), "RoleId" },
            { nameof(RoleClaim.ClaimType), "ClaimType" },
            { nameof(RoleClaim.ClaimValue), "ClaimValue" }
        };
}
