using System.Collections.Generic;
using BaseApiReference.Entities;

namespace BaseApiReference.Metadata;

/// <summary>
/// Metadata for BlackListToken
/// /summary
public class BlackListTokenMetadata : ITableMetadata
{
    public string Name => "BlackListToken";
    public string Schema => "dbo";

    public IReadOnlyDictionary<string, string> ColumnMappings => new Dictionary<string, string>
    {
        { nameof(BlackListToken.TokenHash), "TokenHash" },
        { nameof(BlackListToken.Reason), "Reason" },
        { nameof(BlackListToken.ExpiredAt), "ExpiredAt" },
        { nameof(BlackListToken.CreatedAt), "CreatedAt" },
        { nameof(BlackListToken.CreatedBy), "CreatedBy" },
        { nameof(BlackListToken.User), "User" },
        { nameof(BlackListToken.UserId), "UserId" }
    };
}