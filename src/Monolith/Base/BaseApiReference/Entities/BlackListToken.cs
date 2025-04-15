using System;
using BaseApiReference.Base;

namespace BaseApiReference.Entities;

/// <summary>
/// Represent the "BlackListToken" table.
/// </summary>
public class BlackListToken : BaseEntity<long>, ICreatedEntity
{
    public string TokenHash { get; set; }

    public string Reason { get; set; }

    public DateTime ExpiredAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public long CreatedBy { get; set; }

    // Navigation properties.
    public User User { get; set; }

    public long UserId { get; set; }
}
