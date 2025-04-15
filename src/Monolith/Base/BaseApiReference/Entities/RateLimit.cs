using System;
using BaseApiReference.Base;

namespace BaseApiReference.Entities;

/// <summary>
/// Represent the "RateLimit" table.
/// </summary>
public class RateLimit : BaseEntity<long>
{
    public long? UserId { get; set; }

    public string IpAddress { get; set; }

    public string Endpoint { get; set; }

    public int Count { get; set; }

    public DateTime PeriodStart { get; set; }

    // Navigation
    public User User { get; set; }
}
