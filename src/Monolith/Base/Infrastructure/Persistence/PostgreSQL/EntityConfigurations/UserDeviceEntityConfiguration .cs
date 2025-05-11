using BaseApiReference.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.PostgreSQL.EntityConfigurations;

/// <summary>
/// Represents configuration of "UserDevices" table.
/// </summary>
internal class UserDeviceEntityConfiguration : IEntityTypeConfiguration<UserDevice>
{
    public void Configure(EntityTypeBuilder<UserDevice> builder)
    {
        builder.ToTable(
            name: $"{nameof(UserDevice)}s",
            buildAction: table => table.HasComment(comment: "Contain back up code records.")
        );
    }
}
