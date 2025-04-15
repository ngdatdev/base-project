using System;
using Microsoft.AspNetCore.Identity;

namespace BaseApiReference.Entities;

/// <summary>
///  Represent the "UserTokens" table.
/// </summary>
public class UserToken : IdentityUserToken<long>
{
    public DateTime ExpiredAt { get; set; }

    // Navigation properties.
    public User User { get; set; }
}
