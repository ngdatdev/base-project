using BaseApiReference.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.PostgreSQL.EntityConfigurations;

/// <summary>
/// Represents configuration of "RoleClaims" table.
/// </summary>
internal class RoleClaimEntityConfiguration : IEntityTypeConfiguration<RoleClaim>
{
    public void Configure(EntityTypeBuilder<RoleClaim> builder)
    {
        builder.ToTable(
            name: $"{nameof(RoleClaim)}s",
            buildAction: table => table.HasComment(comment: "Contain RoleClaim records.")
        );
    }
}
