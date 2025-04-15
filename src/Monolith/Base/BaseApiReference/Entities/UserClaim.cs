using Microsoft.AspNetCore.Identity;

namespace BaseApiReference.Entities;

/// <summary>
/// Represent the "UserClaims" table.
/// </summary>
public class UserClaim : IdentityUserClaim<long> { }
