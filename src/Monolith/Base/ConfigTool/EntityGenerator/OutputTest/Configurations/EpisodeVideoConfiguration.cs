using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YourProject.Entities;

namespace YourProject.Data.Configurations
{
    /// <summary>
    /// Entity Configuration for EpisodeVideo
    /// </summary>
    public class EpisodeVideoConfiguration : IEntityTypeConfiguration<EpisodeVideo>
    {
        public void Configure(EntityTypeBuilder<EpisodeVideo> builder)
        {
            // Table name
            builder.ToTable("EpisodeVideo");

            // Primary Key
            builder.HasKey(e => e.Id);

            // Properties Configuration

            builder.Property(e => e.Id)
                .HasColumnName("Id")
                .HasColumnType("INT")
                .IsRequired(true)
;
            builder.Property(e => e.EpisodeId)
                .HasColumnName("EpisodeId")
                .HasColumnType("INT")
                .IsRequired(true)
;
            builder.Property(e => e.ServerId)
                .HasColumnName("ServerId")
                .HasColumnType("INT")
                .IsRequired(true)
;
            builder.Property(e => e.QualityId)
                .HasColumnName("QualityId")
                .HasColumnType("INT")
                .IsRequired(true)
;
            builder.Property(e => e.VideoUrl)
                .HasColumnName("VideoUrl")
                .HasColumnType("NVARCHAR(500)")
                .IsRequired(true)
;
            builder.Property(e => e.SubtitleUrl)
                .HasColumnName("SubtitleUrl")
                .HasColumnType("NVARCHAR(500)")
                .IsRequired(false)
;
            builder.Property(e => e.Language)
                .HasColumnName("Language")
                .HasColumnType("NVARCHAR(10)")
                .IsRequired(false)
                .HasDefaultValue(vi)
;
            builder.Property(e => e.IsActive)
                .HasColumnName("IsActive")
                .HasColumnType("BIT")
                .IsRequired(false)
                .HasDefaultValue(true)
;
            builder.Property(e => e.CreatedAt)
                .HasColumnName("CreatedAt")
                .HasColumnType("DATETIME2")
                .IsRequired(false)
                .HasDefaultValueSql("GETDATE()")
;
            // Indexes
            // TODO: Add indexes based on your requirements
        }
    }
}
