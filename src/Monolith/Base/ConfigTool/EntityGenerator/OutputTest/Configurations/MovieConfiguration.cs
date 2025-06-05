using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YourProject.Entities;

namespace YourProject.Data.Configurations
{
    /// <summary>
    /// Entity Configuration for Movie
    /// </summary>
    public class MovieConfiguration : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            // Table name
            builder.ToTable("Movie");

            // Primary Key
            builder.HasKey(e => e.Id);

            // Properties Configuration

            builder.Property(m => m.Id)
                .HasColumnName("Id")
                .HasColumnType("INT")
                .IsRequired(true)
;
            builder.Property(m => m.Title)
                .HasColumnName("Title")
                .HasColumnType("NVARCHAR(255)")
                .IsRequired(true)
;
            builder.Property(m => m.OriginalTitle)
                .HasColumnName("OriginalTitle")
                .HasColumnType("NVARCHAR(255)")
                .IsRequired(false)
;
            builder.Property(m => m.Slug)
                .HasColumnName("Slug")
                .HasColumnType("NVARCHAR(255)")
                .IsRequired(true)
;
            builder.Property(m => m.Description)
                .HasColumnName("Description")
                .HasColumnType("NTEXT")
                .IsRequired(false)
;
            builder.Property(m => m.PosterUrl)
                .HasColumnName("PosterUrl")
                .HasColumnType("NVARCHAR(255)")
                .IsRequired(false)
;
            builder.Property(m => m.BannerUrl)
                .HasColumnName("BannerUrl")
                .HasColumnType("NVARCHAR(255)")
                .IsRequired(false)
;
            builder.Property(m => m.TrailerUrl)
                .HasColumnName("TrailerUrl")
                .HasColumnType("NVARCHAR(255)")
                .IsRequired(false)
;
            builder.Property(m => m.Duration)
                .HasColumnName("Duration")
                .HasColumnType("INT")
                .IsRequired(false)
;
            builder.Property(m => m.ReleaseDate)
                .HasColumnName("ReleaseDate")
                .HasColumnType("DATE")
                .IsRequired(false)
;
            builder.Property(m => m.ImdbRating)
                .HasColumnName("ImdbRating")
                .HasColumnType("DECIMAL(3,1)")
                .IsRequired(false)
;
            builder.Property(m => m.OurRating)
                .HasColumnName("OurRating")
                .HasColumnType("DECIMAL(3,1)")
                .IsRequired(false)
;
            builder.Property(m => m.ViewCount)
                .HasColumnName("ViewCount")
                .HasColumnType("INT")
                .IsRequired(false)
                .HasDefaultValue(0)
;
            builder.Property(m => m.Status)
                .HasColumnName("Status")
                .HasColumnType("MOVIES_STATUS_ENUM")
                .IsRequired(false)
                .HasDefaultValue(now_showing)
;
            builder.Property(m => m.IsPremium)
                .HasColumnName("IsPremium")
                .HasColumnType("BIT")
                .IsRequired(false)
                .HasDefaultValue(false)
;
            builder.Property(m => m.CountryId)
                .HasColumnName("CountryId")
                .HasColumnType("INT")
                .IsRequired(false)
;
            builder.Property(m => m.CreatedAt)
                .HasColumnName("CreatedAt")
                .HasColumnType("DATETIME2")
                .IsRequired(false)
                .HasDefaultValueSql("GETDATE()")
;
            builder.Property(m => m.UpdatedAt)
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
