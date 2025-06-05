using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YourProject.Entities;

namespace YourProject.Data.Configurations
{
    /// <summary>
    /// Entity Configuration for Rating
    /// </summary>
    public class RatingConfiguration : IEntityTypeConfiguration<Rating>
    {
        public void Configure(EntityTypeBuilder<Rating> builder)
        {
            // Table name
            builder.ToTable("Rating");

            // Primary Key
            builder.HasKey(e => e.Id);

            // Properties Configuration

            builder.Property(r => r.Id)
                .HasColumnName("Id")
                .HasColumnType("INT")
                .IsRequired(true)
;
            builder.Property(r => r.UserId)
                .HasColumnName("UserId")
                .HasColumnType("INT")
                .IsRequired(true)
;
            builder.Property(r => r.MovieId)
                .HasColumnName("MovieId")
                .HasColumnType("INT")
                .IsRequired(false)
;
            builder.Property(r => r.SeriesId)
                .HasColumnName("SeriesId")
                .HasColumnType("INT")
                .IsRequired(false)
;
            builder.Property(r => r.Rating)
                .HasColumnName("Rating")
                .HasColumnType("INT")
                .IsRequired(true)
;
            builder.Property(r => r.Review)
                .HasColumnName("Review")
                .HasColumnType("NTEXT")
                .IsRequired(false)
;
            builder.Property(r => r.CreatedAt)
                .HasColumnName("CreatedAt")
                .HasColumnType("DATETIME2")
                .IsRequired(false)
                .HasDefaultValueSql("GETDATE()")
;
            builder.Property(r => r.UpdatedAt)
                .HasColumnName("UpdatedAt")
                .HasColumnType("DATETIME2")
                .IsRequired(false)
                .HasDefaultValueSql("GETDATE()")
;
            // Indexes
            // TODO: Add indexes based on your requirements
        }
    }
}
