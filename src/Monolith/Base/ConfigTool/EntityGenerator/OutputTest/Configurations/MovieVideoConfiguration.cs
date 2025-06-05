using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YourProject.Entities;

namespace YourProject.Data.Configurations
{
    /// <summary>
    /// Entity Configuration for MovieVideo
    /// </summary>
    public class MovieVideoConfiguration : IEntityTypeConfiguration<MovieVideo>
    {
        public void Configure(EntityTypeBuilder<MovieVideo> builder)
        {
            // Table name
            builder.ToTable("MovieVideo");

            // Primary Key
            builder.HasKey(e => e.Id);

            // Properties Configuration

            builder.Property(m => m.Id)
                .HasColumnName("Id")
                .HasColumnType("INT")
                .IsRequired(true)
;
            builder.Property(m => m.MovieId)
                .HasColumnName("MovieId")
                .HasColumnType("INT")
                .IsRequired(true)
;
            builder.Property(m => m.ServerId)
                .HasColumnName("ServerId")
                .HasColumnType("INT")
                .IsRequired(true)
;
            builder.Property(m => m.QualityId)
                .HasColumnName("QualityId")
                .HasColumnType("INT")
                .IsRequired(true)
;
            builder.Property(m => m.VideoUrl)
                .HasColumnName("VideoUrl")
                .HasColumnType("NVARCHAR(500)")
                .IsRequired(true)
;
            builder.Property(m => m.SubtitleUrl)
                .HasColumnName("SubtitleUrl")
                .HasColumnType("NVARCHAR(500)")
                .IsRequired(false)
;
            builder.Property(m => m.Language)
                .HasColumnName("Language")
                .HasColumnType("NVARCHAR(10)")
                .IsRequired(false)
                .HasDefaultValue(vi)
;
            builder.Property(m => m.IsActive)
                .HasColumnName("IsActive")
                .HasColumnType("BIT")
                .IsRequired(false)
                .HasDefaultValue(true)
;
            builder.Property(m => m.CreatedAt)
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
