using BaseApiReference.Base;
using BaseApiReference.Enum;

namespace BaseApiReference.Entities;

/// <summary>
/// Represent the "UserTwoFactor" table.
/// </summary>
public class UserTwoFactor : BaseEntity<long>
{
    public string SecretKey { get; set; }

    public string TwoFAMethod { get; set; }

    public bool IsEnabled { get; set; }

    public User User { get; set; }

    public long UserId { get; set; }
}
