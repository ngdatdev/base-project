using System.Collections.Generic;
using BaseApiReference.Entities;

namespace BaseApiReference.Metadata;

/// <summary>
/// Metadata for Role
/// </summary>
public class RoleMetadata : ITableMetadata
{
    public string Name => "Role";
    public string Schema => "dbo";

    public IReadOnlyDictionary<string, string> ColumnMappings =>
        new Dictionary<string, string>
        {
            { nameof(Role.Id), "Id" },
            { nameof(Role.Name), "Name" },
            { nameof(Role.NormalizedName), "NormalizedName" },
            { nameof(Role.ConcurrencyStamp), "ConcurrencyStamp" }
        };
}
