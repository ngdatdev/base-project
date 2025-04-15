using BaseApiReference.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.PostgreSQL.EntityConfigurations;

internal class RateLimitEntityConfiguration : IEntityTypeConfiguration<RateLimit>
{
    public void Configure(EntityTypeBuilder<RateLimit> builder)
    {
        builder.ToTable(
            name: $"{nameof(RateLimit)}s",
            buildAction: table => table.HasComment(comment: "Contain rate limit records.")
        );
    }
}
