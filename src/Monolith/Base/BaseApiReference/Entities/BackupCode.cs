using BaseApiReference.Base;

namespace BaseApiReference.Entities;

/// <summary>
/// Represent the "BackupCode" table.
/// </summary>
public class BackupCode : BaseEntity<long>
{
    public string Code { get; set; }

    public string IsUsed { get; set; }

    // Navigation properties.
    public User User { get; set; }

    public long UserId { get; set; }
}
