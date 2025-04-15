using System;
using BaseApiReference.Base;

namespace BaseApiReference.Entities;

/// <summary>
/// Represent the "PasswordHistory" table.
/// </summary>
public class PasswordHistory : BaseEntity<long>
{
    public long UserId { get; set; }

    public string PasswordHash { get; set; }

    public DateTime CreatedAt { get; set; }

    // Navigation
    public User User { get; set; }
}
