using BaseApiReference.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.PostgreSQL.EntityConfigurations;

/// <summary>
/// Represents configuration of "UserTokens" table.
/// </summary>
internal class UserTokenEntityConfiguration : IEntityTypeConfiguration<UserToken>
{
    public void Configure(EntityTypeBuilder<UserToken> builder)
    {
        builder.ToTable(
            name: $"{nameof(UserToken)}s",
            buildAction: table => table.HasComment(comment: "Contain back up code records.")
        );
    }
}
