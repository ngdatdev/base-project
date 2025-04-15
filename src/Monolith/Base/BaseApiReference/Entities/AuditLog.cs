using System;
using BaseApiReference.Base;

namespace BaseApiReference.Entities;

/// <summary>
/// Represent the "AuditLog" table.
/// </summary>
public class AuditLog : BaseEntity<long>
{
    public long? UserId { get; set; }

    public string Action { get; set; }

    public string Data { get; set; }

    public DateTime CreatedAt { get; set; }

    public string IpAddress { get; set; }

    // Navigation
    public User User { get; set; }
}
