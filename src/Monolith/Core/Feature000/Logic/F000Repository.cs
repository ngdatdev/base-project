using System.Linq;
using BaseApiReference.Entities;
using Infrastructure.Persistence.PostgreSQL.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Feature000.Logic;

/// <summary>
/// Implementation of F000 Repository
/// </summary>
public sealed class F000Repository : IF000Repository
{
    private readonly AppDbContext _context;
    private readonly DbSet<User> _users;

    public F000Repository(AppDbContext context)
    {
        _context = context;
        _users = _context.Set<User>();
    }

    public bool IsUserFoundByUsername(string username)
    {
        return _users.Any(x => x.UserName == username);
    }
}
