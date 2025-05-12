using BaseApiReference.Base;

namespace BaseApiReference.Enum;

/// <summary>
/// TwoFA Method Enum.
/// </summary>
public sealed class TwoFAMethod : SmartEnum<TwoFAMethod, string>
{
    public static readonly TwoFAMethod None = new("None", "None");
    public static readonly TwoFAMethod Email = new("Email", "Email");
    public static readonly TwoFAMethod Phone = new("Phone", "Phone");
    public static readonly TwoFAMethod Backup = new("Backup", "Backup");

    private TwoFAMethod(string value, string name)
        : base(value, name) { }
}
