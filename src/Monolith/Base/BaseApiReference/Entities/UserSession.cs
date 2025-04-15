using System;
using BaseApiReference.Base;

namespace BaseApiReference.Entities;

/// <summary>
/// Represent the "UserSession" table.
/// </summary>
public class UserSession : BaseEntity<long>
{
    public DateTime ExperiedDate { get; set; }

    public string RefreshToken { get; set; }

    public string DeviderId { get; set; }

    public string IpAddress { get; set; }

    public DateTime LastActive { get; set; }

    // Navigation properties.
    public User User { get; set; }
}
