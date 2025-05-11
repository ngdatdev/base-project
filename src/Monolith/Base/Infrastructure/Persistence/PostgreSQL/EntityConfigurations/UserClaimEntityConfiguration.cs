using BaseApiReference.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.PostgreSQL.EntityConfigurations;

/// <summary>
/// Represents configuration of "UserClaims" table.
/// </summary>
internal class UserClaimEntityConfiguration : IEntityTypeConfiguration<UserClaim>
{
    public void Configure(EntityTypeBuilder<UserClaim> builder)
    {
        builder.ToTable(
            name: $"{nameof(UserClaim)}s",
            buildAction: table => table.HasComment(comment: "Contain UserClaim records.")
        );
    }
}
