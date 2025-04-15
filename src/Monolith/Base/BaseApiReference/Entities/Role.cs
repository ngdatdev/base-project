using System;
using Microsoft.AspNetCore.Identity;

namespace BaseApiReference.Entities;

/// <summary>
/// Represent the "Roles" table.
/// </summary>
public class Role : IdentityRole<long> { }
