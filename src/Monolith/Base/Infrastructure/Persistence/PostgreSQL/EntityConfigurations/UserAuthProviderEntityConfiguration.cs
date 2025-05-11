using BaseApiReference.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.PostgreSQL.EntityConfigurations;

/// <summary>
/// Represents configuration of "UserAuthProviders" table.
/// </summary>
internal class UserAuthProviderEntityConfiguration : IEntityTypeConfiguration<UserAuthProvider>
{
    public void Configure(EntityTypeBuilder<UserAuthProvider> builder)
    {
        builder.ToTable(
            name: $"{nameof(UserAuthProvider)}s",
            buildAction: table => table.HasComment(comment: "Contain UserAuthProvider records.")
        );
    }
}
