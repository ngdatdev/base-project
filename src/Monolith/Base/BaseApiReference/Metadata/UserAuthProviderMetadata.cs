using System.Collections.Generic;
using BaseApiReference.Entities;

namespace BaseApiReference.Metadata;

/// <summary>
/// Metadata for UserAuthProvider
/// /summary
public class UserAuthProviderMetadata : ITableMetadata
{
    public string Name => "UserAuthProvider";
    public string Schema => "dbo";

    public IReadOnlyDictionary<string, string> ColumnMappings => new Dictionary<string, string>
    {

    };
}