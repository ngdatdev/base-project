using System.Collections.Generic;
using BaseApiReference.Entities;

namespace BaseApiReference.Metadata;

/// <summary>
/// Metadata for UserRole
/// </summary>
public class UserRoleMetadata : ITableMetadata
{
    public string Name => "UserRole";
    public string Schema => "dbo";

    public IReadOnlyDictionary<string, string> ColumnMappings =>
        new Dictionary<string, string>
        {
            { nameof(UserRole.UserId), "UserId" },
            { nameof(UserRole.RoleId), "RoleId" }
            // Navigation properties are typically not mapped as columns
            // { nameof(UserRole.User), "User" },
            // { nameof(UserRole.Role), "Role" }
        };
}
