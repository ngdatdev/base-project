using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YourProject.Entities;

namespace YourProject.Data.Configurations
{
    /// <summary>
    /// Entity Configuration for Series
    /// </summary>
    public class SeriesConfiguration : IEntityTypeConfiguration<Series>
    {
        public void Configure(EntityTypeBuilder<Series> builder)
        {
            // Table name
            builder.ToTable("Series");

            // Primary Key
            builder.HasKey(e => e.Id);

            // Properties Configuration

            builder.Property(s => s.Id)
                .HasColumnName("Id")
                .HasColumnType("INT")
                .IsRequired(true)
;
            builder.Property(s => s.Title)
                .HasColumnName("Title")
                .HasColumnType("NVARCHAR(255)")
                .IsRequired(true)
;
            builder.Property(s => s.OriginalTitle)
                .HasColumnName("OriginalTitle")
                .HasColumnType("NVARCHAR(255)")
                .IsRequired(false)
;
            builder.Property(s => s.Slug)
                .HasColumnName("Slug")
                .HasColumnType("NVARCHAR(255)")
                .IsRequired(true)
;
            builder.Property(s => s.Description)
                .HasColumnName("Description")
                .HasColumnType("NTEXT")
                .IsRequired(false)
;
            builder.Property(s => s.PosterUrl)
                .HasColumnName("PosterUrl")
                .HasColumnType("NVARCHAR(255)")
                .IsRequired(false)
;
            builder.Property(s => s.BannerUrl)
                .HasColumnName("BannerUrl")
                .HasColumnType("NVARCHAR(255)")
                .IsRequired(false)
;
            builder.Property(s => s.TrailerUrl)
                .HasColumnName("TrailerUrl")
                .HasColumnType("NVARCHAR(255)")
                .IsRequired(false)
;
            builder.Property(s => s.TotalSeasons)
                .HasColumnName("TotalSeasons")
                .HasColumnType("INT")
                .IsRequired(false)
                .HasDefaultValue(1)
;
            builder.Property(s => s.TotalEpisodes)
                .HasColumnName("TotalEpisodes")
                .HasColumnType("INT")
                .IsRequired(false)
                .HasDefaultValue(0)
;
            builder.Property(s => s.ReleaseDate)
                .HasColumnName("ReleaseDate")
                .HasColumnType("DATE")
                .IsRequired(false)
;
            builder.Property(s => s.EndDate)
                .HasColumnName("EndDate")
                .HasColumnType("DATE")
                .IsRequired(false)
;
            builder.Property(s => s.ImdbRating)
                .HasColumnName("ImdbRating")
                .HasColumnType("DECIMAL(3,1)")
                .IsRequired(false)
;
            builder.Property(s => s.OurRating)
                .HasColumnName("OurRating")
                .HasColumnType("DECIMAL(3,1)")
                .IsRequired(false)
;
            builder.Property(s => s.ViewCount)
                .HasColumnName("ViewCount")
                .HasColumnType("INT")
                .IsRequired(false)
                .HasDefaultValue(0)
;
            builder.Property(s => s.Status)
                .HasColumnName("Status")
                .HasColumnType("SERIES_STATUS_ENUM")
                .IsRequired(false)
                .HasDefaultValue(ongoing)
;
            builder.Property(s => s.IsPremium)
                .HasColumnName("IsPremium")
                .HasColumnType("BIT")
                .IsRequired(false)
                .HasDefaultValue(false)
;
            builder.Property(s => s.CountryId)
                .HasColumnName("CountryId")
                .HasColumnType("INT")
                .IsRequired(false)
;
            builder.Property(s => s.CreatedAt)
                .HasColumnName("CreatedAt")
                .HasColumnType("DATETIME2")
                .IsRequired(false)
                .HasDefaultValueSql("GETDATE()")
;
            builder.Property(s => s.UpdatedAt)
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
