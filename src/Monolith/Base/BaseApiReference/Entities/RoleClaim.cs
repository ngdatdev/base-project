using Microsoft.AspNetCore.Identity;

namespace BaseApiReference.Entities;

/// <summary>
/// Represent the "RoleClaims" table.
/// </summary>
public class RoleClaim : IdentityRoleClaim<long> { }
