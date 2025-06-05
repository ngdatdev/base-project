using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YourProject.Entities;

namespace YourProject.Data.Configurations
{
    /// <summary>
    /// Entity Configuration for Banner
    /// </summary>
    public class BannerConfiguration : IEntityTypeConfiguration<Banner>
    {
        public void Configure(EntityTypeBuilder<Banner> builder)
        {
            // Table name
            builder.ToTable("Banner");

            // Primary Key
            builder.HasKey(e => e.Id);

            // Properties Configuration

            builder.Property(b => b.Id)
                .HasColumnName("Id")
                .HasColumnType("INT")
                .IsRequired(true)
;
            builder.Property(b => b.Title)
                .HasColumnName("Title")
                .HasColumnType("NVARCHAR(255)")
                .IsRequired(true)
;
            builder.Property(b => b.ImageUrl)
                .HasColumnName("ImageUrl")
                .HasColumnType("NVARCHAR(255)")
                .IsRequired(true)
;
            builder.Property(b => b.LinkUrl)
                .HasColumnName("LinkUrl")
                .HasColumnType("NVARCHAR(255)")
                .IsRequired(false)
;
            builder.Property(b => b.MovieId)
                .HasColumnName("MovieId")
                .HasColumnType("INT")
                .IsRequired(false)
;
            builder.Property(b => b.SeriesId)
                .HasColumnName("SeriesId")
                .HasColumnType("INT")
                .IsRequired(false)
;
            builder.Property(b => b.Position)
                .HasColumnName("Position")
                .HasColumnType("INT")
                .IsRequired(false)
                .HasDefaultValue(1)
;
            builder.Property(b => b.IsActive)
                .HasColumnName("IsActive")
                .HasColumnType("BIT")
                .IsRequired(false)
                .HasDefaultValue(true)
;
            builder.Property(b => b.StartDate)
                .HasColumnName("StartDate")
                .HasColumnType("DATE")
                .IsRequired(false)
;
            builder.Property(b => b.EndDate)
                .HasColumnName("EndDate")
                .HasColumnType("DATE")
                .IsRequired(false)
;
            builder.Property(b => b.CreatedAt)
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
