using BaseApiReference.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.PostgreSQL.EntityConfigurations;

/// <summary>
/// Represents configuration of "UserLogins" table.
/// </summary>
internal class UserLoginEntityConfiguration : IEntityTypeConfiguration<UserLogin>
{
    public void Configure(EntityTypeBuilder<UserLogin> builder)
    {
        builder.ToTable(
            name: $"{nameof(UserLogin)}s",
            buildAction: table => table.HasComment(comment: "Contain UserLogin records.")
        );
    }
}
