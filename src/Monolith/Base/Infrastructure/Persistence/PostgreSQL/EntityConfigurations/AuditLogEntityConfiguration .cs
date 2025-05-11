using BaseApiReference.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.PostgreSQL.EntityConfigurations;

/// <summary>
/// Represents configuration of "AuditLogs" table.
/// </summary>
internal class AuditLogEntityConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable(
            name: $"{nameof(AuditLog)}s",
            buildAction: table => table.HasComment(comment: "Contain AuditLog code records.")
        );
    }
}
