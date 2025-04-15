using BaseApiReference.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.PostgreSQL.EntityConfigurations;

/// <summary>
/// Represents configuration of "BackupCodes" table.
/// </summary>
internal class BackupCodeEntityConfiguration : IEntityTypeConfiguration<BackupCode>
{
    public void Configure(EntityTypeBuilder<BackupCode> builder)
    {
        builder.ToTable(
            name: $"{nameof(BackupCode)}s",
            buildAction: table => table.HasComment(comment: "Contain back up code records.")
        );
    }
}
