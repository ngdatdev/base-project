using BaseApiReference.Base;

namespace BaseApiReference.Enum;

/// <summary>
/// Account Status Enum.
/// </summary>
public sealed class AccountStatus : SmartEnum<AccountStatus, string>
{
    public static readonly AccountStatus Active = new(nameof(Active), "Active");
    public static readonly AccountStatus Inactive = new(nameof(Inactive), "Inactive");
    public static readonly AccountStatus Locked = new(nameof(Locked), "Locked");
    public static readonly AccountStatus Deleted = new(nameof(Deleted), "Deleted");

    private AccountStatus(string value, string name)
        : base(value, name) { }
}
