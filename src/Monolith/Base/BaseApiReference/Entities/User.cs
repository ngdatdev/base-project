using System;
using BaseApiReference.Base;
using BaseApiReference.Enum;
using Microsoft.AspNetCore.Identity;

namespace BaseApiReference.Entities;

/// <summary>
/// Represent the "Users" table.
/// </summary>
public class User : IdentityUser<long>, ICreatedEntity, ITemporarilyRemovedEntity, IUpdatedEntity
{
    // Navigation properties.
    public string FullName { get; set; }

    public string Avatar { get; set; }

    public string Status { get; set; }

    public DateTime LastLogin { get; set; }

    public string PreferedTwoFAMethod { get; set; }

    public DateTime CreatedAt { get; set; }

    public long CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public long? UpdatedBy { get; set; }

    public DateTime? RemovedAt { get; set; }

    public long? RemovedBy { get; set; }
}
