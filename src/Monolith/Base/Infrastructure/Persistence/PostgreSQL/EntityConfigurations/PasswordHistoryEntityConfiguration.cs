using BaseApiReference.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.PostgreSQL.EntityConfigurations;

/// <summary>
/// Represents configuration of "PasswordHistory" table.
/// </summary>
internal class PasswordHistoryEntityConfiguration : IEntityTypeConfiguration<PasswordHistory>
{
    public void Configure(EntityTypeBuilder<PasswordHistory> builder)
    {
        builder.ToTable(
            name: $"PasswordHistories",
            buildAction: table => table.HasComment(comment: "Contain password history records.")
        );
    }
}
