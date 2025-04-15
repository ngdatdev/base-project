using Microsoft.AspNetCore.Identity;

namespace BaseApiReference.Entities;

/// <summary>
/// Represent the "UserLogins" table.
/// </summary>
public class UserLogin : IdentityUserLogin<long> { }
