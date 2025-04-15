using BaseApiReference.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.PostgreSQL.EntityConfigurations;

/// <summary>
/// Represents configuration of "BlackListTokens" table.
/// </summary>
internal class BlackListTokenEntityConfiguration : IEntityTypeConfiguration<BlackListToken>
{
    public void Configure(EntityTypeBuilder<BlackListToken> builder)
    {
        builder.ToTable(
            name: $"{nameof(BlackListToken)}s",
            buildAction: table => table.HasComment(comment: "Contain black list token records.")
        );
    }
}
