using Microsoft.AspNetCore.Identity;

namespace BaseApiReference.Entities;

/// <summary>
/// Represent the "UserRoles" table.
/// </summary>
public class UserRole : IdentityUserRole<long>
{
    // Navigation properties.
    public User User { get; set; }

    public Role Role { get; set; }
}
