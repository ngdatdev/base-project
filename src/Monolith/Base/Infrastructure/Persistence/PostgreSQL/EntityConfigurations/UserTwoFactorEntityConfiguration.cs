using BaseApiReference.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.PostgreSQL.EntityConfigurations;

/// <summary>
/// Represents configuration of "UserTwoFactors" table.
/// </summary>
internal class UserTwoFactorEntityConfiguration : IEntityTypeConfiguration<UserTwoFactor>
{
    public void Configure(EntityTypeBuilder<UserTwoFactor> builder)
    {
        builder.ToTable(
            name: $"{nameof(UserTwoFactor)}s",
            buildAction: table => table.HasComment(comment: "Contain UserTwoFactor records.")
        );
    }
}
