using BaseApiReference.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.PostgreSQL.EntityConfigurations;

/// <summary>
/// Represents configuration of "UserSessions" table.
/// </summary>
internal class UserSessionEntityConfiguration : IEntityTypeConfiguration<UserSession>
{
    public void Configure(EntityTypeBuilder<UserSession> builder)
    {
        builder.ToTable(
            name: $"{nameof(UserSession)}s",
            buildAction: table => table.HasComment(comment: "Contain back up code records.")
        );
    }
}
