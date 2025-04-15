using System;
using BaseApiReference.Base;

namespace BaseApiReference.Entities;

/// <summary>
/// Represent the "UserDevice" table.
/// </summary>
public class UserDevice : BaseEntity<long>
{
    public long UserId { get; set; }

    public string DeviceId { get; set; }

    public string DeviceType { get; set; }

    public string DeviceName { get; set; }

    public DateTime LastUsedAt { get; set; }

    // Navigation
    public User User { get; set; }
}
