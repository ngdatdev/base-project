using BaseApiReference.Entities;
using Infrastructure.Persistence.PostgreSQL.Common;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.PostgreSQL.DbContext;

/// <summary>
/// Implementation of database context.
/// </summary>
public class AppDbContext : IdentityDbContext<User, Role, long>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    /// <summary>
    /// Configure tables and seed initial data here.
    /// </summary>
    /// <param name="builder">
    /// Model builder access the database.
    /// </param>

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder: builder);

        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        RemoveAspNetPrefixInIdentityTable(builder: builder);

        builder.HasDefaultSchema(Constant.DatabaseSchema);

        InitCaseInsensitiveCollation(builder: builder);
    }

    /// <summary>
    /// Remove "AspNet" prefix in identity table name.
    /// </summary>
    /// <param name="builder">
    /// Model builder access the database.
    /// </param>
    private static void RemoveAspNetPrefixInIdentityTable(ModelBuilder builder)
    {
        const string AspNetPrefix = "AspNet";

        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            var tableName = entityType.GetTableName();

            if (tableName.StartsWith(value: AspNetPrefix))
            {
                entityType.SetTableName(name: tableName[6..]);
            }
        }
    }

    /// <summary>
    /// Create new case insensitive collation.
    /// </summary>
    /// <param name="builder">
    /// Model builder access the database.
    /// </param>
    private static void InitCaseInsensitiveCollation(ModelBuilder builder)
    {
        builder.HasCollation(
            Constant.Collation.CASE_INSENSITIVE,
            Constant.Collation.CASE_INSENSITIVE,
            "icu",
            false
        );
    }
}
